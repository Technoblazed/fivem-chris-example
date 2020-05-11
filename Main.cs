using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;

namespace chrisproject
{
    public class Main : BaseScript
    {
        private bool isSpamEnabled;
        private DateTime _lastSpamTime;

        [Tick]
        internal async Task OnSpamTick()
        {
            if (!isSpamEnabled)
            {
                return;
            }

            if (_lastSpamTime.AddSeconds(10) < DateTime.UtcNow)
            {
                TriggerEvent("chat:addMessage", new
                {
                    args = new[] { $"[^1SPAM^7] ABCDEFGHIJKLMNOPQRSTUVWXYZ." }
                });

                _lastSpamTime = DateTime.UtcNow;
            }

            await Task.FromResult(0);
        }

        [Command("spam")]
        internal void OnSpamCommand(int src, List<object> args, string raw)
        {
            if (args.Count == 1)
            {
                string newState = args[0].ToString().ToLower();

                if (newState == "start" && !isSpamEnabled)
                {
                    isSpamEnabled = !isSpamEnabled;
                    _lastSpamTime = DateTime.UtcNow;

                    TriggerEvent("chat:addMessage", new
                    {
                        args = new[] { $"[^1SPAM^7] Spam enabled, hide yo' kids." }
                    });
                }
                else if (newState == "stop" && isSpamEnabled)
                {
                    isSpamEnabled = !isSpamEnabled;

                    TriggerEvent("chat:addMessage", new
                    {
                        args = new[] { $"[^1SPAM^7] Spam stopped, the world is safe." }
                    });
                }
                else
                {
                    TriggerEvent("chat:addMessage", new
                    {
                        args = new[] { $"[^1SPAM^7] Spam is already {(newState == "start" ? "started" : "stopped")}." }
                    });
                }
            }
            else
            {
                TriggerEvent("chat:addMessage", new
                {
                    args = new[] { $"[^1SPAM^7] Invalid command provided. Usage: /spam [start|stop]." }
                });
            }
        }
    }
}
