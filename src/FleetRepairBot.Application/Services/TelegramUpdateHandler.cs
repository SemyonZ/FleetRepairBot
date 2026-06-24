using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using FleetRepairBot.Application.Interfaces;
using FleetRepairBot.Application.Dto;

namespace FleetRepairBot.Application.Services
{
    public class TelegramUpdateHandler
    {
        private readonly ITelegramClient _tgClient;
        private readonly IRepairRequestService _requestService;
        private readonly ILogger<TelegramUpdateHandler> _logger;

        public TelegramUpdateHandler(ITelegramClient tgClient, IRepairRequestService requestService, ILogger<TelegramUpdateHandler> logger)
        {
            _tgClient = tgClient ?? throw new ArgumentNullException(nameof(tgClient));
            _requestService = requestService ?? throw new ArgumentNullException(nameof(requestService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // This method matches signature used by Telegram.Bot StartReceiving
        public async Task HandleAsync(ITelegramBotClient botClient, Update update, CancellationToken ct)
        {
            if (update == null) return;

            try
            {
                if (update.Type != UpdateType.Message || update.Message == null)
                {
                    _logger.LogDebug("Ignoring non-message update of type {Type}", update.Type);
                    return;
                }

                var msg = update.Message;
                var chatId = msg.Chat.Id;
                var fromId = msg.From?.Id ?? 0;

                if (msg.Text != null)
                {
                    var text = msg.Text.Trim();
                    if (text.StartsWith("/start", StringComparison.OrdinalIgnoreCase))
                    {
                        await _tgClient.SendTextAsync(chatId, "Welcome to FleetRepairBot. Send a text describing the issue or attach a photo with optional caption.", ct);
                        return;
                    }

                    // Treat any other text as a repair report description
                    var dto = new RepairRequestCreateDto
                    {
                        Title = text.Length <= 50 ? text : text.Substring(0, 50),
                        Description = text,
                        ReporterTelegramId = fromId
                    };

                    var created = await _requestService.CreateAsync(dto, ct);

                    await _tgClient.SendTextAsync(chatId, $"Thank you. Repair request received (Id: {created?.Id}). Our team will review it.", ct);
                    return;
                }

                // If photo(s) present, download the largest and create request with optional caption
                if (msg.Photo != null && msg.Photo.Any())
                {
                    var photo = msg.Photo.OrderByDescending(p => p.FileSize ?? 0).First();
                    var fileId = photo.FileId;
                    Stream stream = null;
                    try
                    {
                        stream = await _tgClient.DownloadFileAsync(fileId, ct);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Failed to download photo {FileId}", fileId);
                    }

                    var caption = msg.Caption ?? "Photo report";

                    var dto = new RepairRequestCreateDto
                    {
                        Title = caption.Length <= 50 ? caption : caption.Substring(0, 50),
                        Description = caption,
                        ReporterTelegramId = fromId
                    };

                    var created = await _requestService.CreateAsync(dto, ct);

                    // If we have a photo stream, attach it using service AddPhotoAsync if available (not part of application contract here)
                    if (stream != null)
                    {
                        try
                        {
                            // Try to save via service if method exists (best-effort) using reflection to avoid strong dependency
                            var addPhoto = _requestService.GetType().GetMethod("AddPhotoAsync");
                            if (addPhoto != null)
                            {
                                // signature: AddPhotoAsync(int requestId, Stream photoStream, string fileName, long performedByTelegramId, CancellationToken ct = default)
                                addPhoto.Invoke(_requestService, new object[] { created.Id, stream, $"photo_{created.Id}.jpg", (long)fromId, ct });
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, "Failed to attach photo to request {RequestId}", created.Id);
                        }
                    }

                    await _tgClient.SendTextAsync(chatId, $"Photo received. Repair request created (Id: {created?.Id}).", ct);
                    return;
                }

                // Default fallback
                await _tgClient.SendTextAsync(chatId, "Please send a text description or a photo of the issue.", ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception in TelegramUpdateHandler");
            }
        }
    }
}
