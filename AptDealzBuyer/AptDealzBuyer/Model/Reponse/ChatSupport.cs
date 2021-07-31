using Newtonsoft.Json;
using System;

namespace AptDealzBuyer.Model.Reponse
{
    public class ChatSupport
    {
        [JsonProperty("chatMessageId")]
        public string ChatMessageId { get; set; }

        [JsonProperty("chatId")]
        public string ChatId { get; set; }

        [JsonProperty("isMessageFromSupportTeam")]
        public bool IsMessageFromSupportTeam { get; set; }

        [JsonProperty("chatMessageFromUserName")]
        public string ChatMessageFromUserName { get; set; }

        [JsonProperty("chatMessageFromUserProfileImage")]
        public string ChatMessageFromUserProfileImage { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("chatMessageFromUserId")]
        public string ChatMessageFromUserId { get; set; }

        [JsonProperty("createdDate")]
        public DateTime CreatedDate { get; set; }

        #region [ Extra Properties ]     
        [JsonIgnore]
        public bool IsContact { get; set; } = true;

        [JsonIgnore]
        public bool IsUser { get; set; } = false;
        #endregion
    }
}
