using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace HelpDeskBot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        private string category;
        private string severity;
        private string description;

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        //private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        //{
        //    var activity = await result as Activity;
        //
            // calculate something for us to return
        //    int length = (activity.Text ?? string.Empty).Length;

            // return our reply to the user
        //    await context.PostAsync($"You sent {activity.Text} which was {length} characters");

        //    context.Wait(MessageReceivedAsync);
        //}

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
        await context.PostAsync
            ("Help Desk Bot です。サポートデスク受付チケットの発行を行います。");
        PromptDialog.Text(context, this.DescriptionMessageReceviedAsync, "どんなことにお困りですか？");
        }

        public async Task DescriptionMessageReceviedAsync(IDialogContext context, IAwaitable<string> argument)
        {
            this.description = await argument;
            await context.PostAsync($"承知しました。内容は \"{this.description}\"ですね。");
            context.Done<object>(null);
        }

    }
}