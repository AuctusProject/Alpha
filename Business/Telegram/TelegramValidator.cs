using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.Business.WrapTelegram
{
    public class TelegramValdiator
    {
        public static bool CheckPhoneIsMember(string phoneNumber)
        {
            Telegram.Bot.TelegramBotClient client = new Telegram.Bot.TelegramBotClient("508703489:AAGL4_1v8kGjzB5DMoVnklAAuYo1Vk3-XcA");
            var t = client.GetChatMemberAsync("-1001394089260", 128844089);
            var s = t.Result.Status;
            if (s.ToString().Length > 25)
                return false;
            return true;

            //Status tem que estar como Member;
        }
    }
}
