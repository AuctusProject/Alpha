using Auctus.Util.NotShared;
using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Auctus.Business.Telegram
{
    internal class TelegramValidator
    {
        public static bool CheckPhoneIsMember(string phoneNumber)
        {
            TelegramBotClient client = new TelegramBotClient(Config.TELEGRAM_BOT_TOKEN);
            var sendContactResult = client.SendContactAsync(Config.TELEGRAM_CHAT_ID_FOR_CHECK_PHONE_ID, phoneNumber, "NAME").Result;
            var userId = sendContactResult.Contact.UserId;

            if (userId == 0)
            {
                throw new ArgumentException("Phone number not linked to a Telegram account.");
            }

            var getChatMemberResult = client.GetChatMemberAsync(Config.TELEGRAM_AUCTUS_CHAT_ID, userId).Result;
            
            if (!getChatMemberResult.Status.Equals(ChatMemberStatus.Member) &&
                !getChatMemberResult.Status.Equals(ChatMemberStatus.Creator) &&
                !getChatMemberResult.Status.Equals(ChatMemberStatus.Restricted) &&
                !getChatMemberResult.Status.Equals(ChatMemberStatus.Administrator))
            {
                throw new Exception("User is not a member of Auctus Telegram group.");
            }

            return true;
        }
    }
}
