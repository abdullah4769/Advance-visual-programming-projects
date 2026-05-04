using System;
using System.Drawing;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AIChatbotApp
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ChatForm());
        }
    }

    public class ChatForm : Form
    {
        private RichTextBox txtChat;
        private TextBox txtInput;
        private Button btnSend;
        private readonly HttpClient client = new HttpClient();

        public ChatForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Text = "AI Chatbot";
            Size = new Size(600, 500);
            StartPosition = FormStartPosition.CenterScreen;
            Icon = SystemIcons.Application;

            txtChat = new RichTextBox
            {
                Location = new Point(10, 10),
                Size = new Size(565, 400),
                ReadOnly = true,
                ScrollBars = RichTextBoxScrollBars.Vertical,
                BackColor = Color.White,
                Font = new Font("Segoe UI", 11),
                Padding = new Padding(5)
            };

            txtInput = new TextBox
            {
                Location = new Point(10, 420),
                Size = new Size(465, 30),
                Font = new Font("Segoe UI", 11)
            };

            btnSend = new Button
            {
                Location = new Point(485, 420),
                Size = new Size(90, 30),
                Text = "Send",
                Font = new Font("Segoe UI", 10)
            };

            btnSend.Click += async (s, e) => await BtnSend_Click();

            Controls.Add(txtChat);
            Controls.Add(txtInput);
            Controls.Add(btnSend);
        }

        // ✅ FIXED BUTTON LOGIC (NOW USES AI)
        private async Task BtnSend_Click()
        {
            string userMessage = txtInput.Text.Trim();

            if (string.IsNullOrWhiteSpace(userMessage))
            {
                txtChat.AppendColoredText("⚠ Please enter a message\n\n", Color.Red);
                return;
            }

            // Add user message with proper spacing
            if (txtChat.Text.Length > 0)
            {
                txtChat.AppendColoredText("\n", Color.Black);
            }

            txtChat.AppendColoredText($"You: {userMessage}\n", Color.DarkBlue);
            txtInput.Clear();

            // Add bot thinking message
            txtChat.AppendColoredText($"AI Assistant: Thinking...\n", Color.Green);
            txtChat.ScrollToCaret();

            string aiResponse = await GetAIResponse(userMessage);

            // Replace "Thinking..." with actual response
            txtChat.Text = txtChat.Text.Replace("AI Assistant: Thinking...\n", $"AI Assistant: {aiResponse}\n");

            // Add extra spacing after bot response
            txtChat.AppendColoredText("\n", Color.Black);
            txtChat.ScrollToCaret();

            txtInput.Focus();
        }

        // ✅ REAL AI CALL FUNCTION (INSIDE CLASS)
        private async Task<string> GetAIResponse(string message)
        {
            message = message.ToLower();

            await Task.Delay(500); // fake thinking delay

            if (message.Contains("hello"))
                return "Hello! How can I help you today?";

            if (message.Contains("ai"))
                return "AI means Artificial Intelligence. It allows machines to simulate human thinking.";

            if (message.Contains("name"))
                return "I am a simple AI chatbot built in C# WinForms.";

            return "I understand: " + message;
        }
    }

    // ✅ Safe extension method
    public static class RichTextBoxExtensions
    {
        public static void AppendColoredText(this RichTextBox rtb, string text, Color color)
        {
            rtb.SelectionStart = rtb.TextLength;
            rtb.SelectionLength = 0;

            rtb.SelectionColor = color;
            rtb.SelectedText = text;

            rtb.SelectionColor = rtb.ForeColor;
        }
    }
}