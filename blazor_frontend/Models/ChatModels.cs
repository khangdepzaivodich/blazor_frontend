using System;

namespace blazor_frontend.Models
{
    public class ChatMessageDto
    {
        public string Id { get; set; } = string.Empty;
        public Guid MaPhien { get; set; }
        public Guid SenderID { get; set; }
        public string SenderType { get; set; } = "user";
        public string NoiDung { get; set; } = string.Empty;
        public DateTime ThoiGianGui { get; set; } = DateTime.UtcNow;
        public string TrangThai { get; set; } = "sent";
        public Guid ClientID { get; set; } = Guid.NewGuid();
    }

    public class ChatSessionDto
    {
        public Guid Id { get; set; }
        public Guid UserID { get; set; }
        public DateTime ThoiGianTao { get; set; }
        public string TrangThai { get; set; } = "ACTIVE";
        public string StaffID { get; set; } = string.Empty;
        public string ClientType { get; set; } = "GUEST";
        public DateTime LastTime { get; set; }
        public string LastMessage { get; set; } = string.Empty;
        public string HoTen { get; set; } = string.Empty;
        public string StaffHoTen { get; set; } = string.Empty;
        public int UnreadCount { get; set; }
    }
}
