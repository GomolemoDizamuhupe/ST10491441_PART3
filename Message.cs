using System.Windows.Media;

namespace PART2_POE_
{
    public class Message
    {
        public string Time { get; set; }
        public string Text { get; set; }
        public string Sender { get; set; }

        // Stores the colour of the message bubble
        public Brush MessageColor { get; set; }
    }
}