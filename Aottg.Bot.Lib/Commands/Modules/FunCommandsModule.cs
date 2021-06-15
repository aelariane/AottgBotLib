using System;

namespace AottgBotLib.Commands.Modules
{
    /// <summary>
    /// Contains basic commands for showcase
    /// </summary>
    public class FunCommandsModule : CommandModule
    {
        private Random _rnd = new Random();

        /// <summary>
        /// Sends random number in range 1-100 to chat
        /// </summary>
        /// <param name="ctx"></param>
        [Command("rnd")]
        [Description("Generates random number in range of 1-100")]
        public void RandomCommand(CommandContext ctx)
        {
            ctx.Client.SendChatMessage("You rolled: " + _rnd.Next(1, 101));
        }

        /// <summary>
        /// Gives simple answer to question in fun form
        /// </summary>
        /// <param name="ctx"></param>
        [Command("ask")]
        [Description("Gives a reply to your question")]
        public void AskCommand(CommandContext ctx)
        {
            int selection = _rnd.Next(0, 6);
            string reply = string.Empty;
            switch (selection)
            {
                case 0:
                    reply = "Absolutely Yes.";
                    break;
                case 1:
                    reply = "More yes then no.";
                    break;
                case 2:
                    reply = "I do not know. This is too hard for meeeee ;~;";
                    break;
                case 3:
                    reply = "I think.. Uh.. No?";
                    break;
                case 4:
                    reply = "Absolutely No.";
                    break;
                case 5:
                    reply = "How dare you.";
                    break;
            }
            if (selection != 2)
            {
                int kek = _rnd.Next(0, 2);
                if (kek == 1)
                {
                    kek = _rnd.Next(0, 5);
                    string addMeme = "";
                    switch (kek)
                    {
                        case 0:
                            addMeme = "UwU";
                            break;

                        case 1:
                            addMeme = "OwO";
                            break;

                        case 2:
                            addMeme = "3w3";
                            break;
                        case 3:
                            addMeme = "QwQ";
                            break;
                        case 4:
                            addMeme = "Meow~";
                            break;
                    }

                    reply += " " + addMeme;
                }
            }
            ctx.Client.SendChatMessage(reply);
        }
    }
}
