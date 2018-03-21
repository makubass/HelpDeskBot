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
            await context.PostAsync("Help Desk Bot です。サポートデスク受付チケットの発行を行います。");
            PromptDialog.Text(context, this.DescriptionMessageReceivedAsync, "どんなことにお困りですか？");
        }

        public async Task DescriptionMessageReceivedAsync(IDialogContext context, IAwaitable<string> argument)
        {
            this.description = await argument;
            var severity = new string[] { "high", "normal", "low" };
            PromptDialog.Choice(context, this.SeverityMessageReceivedAsync, severity, 
                "この問題の重要度を入力してください");
        }

        public async Task SeverityMessageReceivedAsync(IDialogContext context, IAwaitable<string> argument)
        {
            this.severity = await argument;
            PromptDialog.Text(context, this.CategoryMessageReceivedAsync, 
                "この問題のカテゴリーを以下から選んで入力してください \n\n" + 
                "software,hardware, networking, security, other");
        }

        public async Task CategoryMessageReceivedAsync(IDialogContext context, IAwaitable<string> argument)
        {
            this.category = await argument;
            var text = "承知しました。 \n\n"
                + $"重要度: \"{this.severity}\"、カテゴリー: \"{this.category}\" "
                + "でサポートチケットを発行します。 \n\n"
                + $"詳細: \"{this.description}\" \n\n"
                + "以上の内容で宜しいでしょうか？";

            PromptDialog.Confirm(context, this.IssueConfirmedMessageReceivedAsync, text);
        }

        public async Task IssueConfirmedMessageReceivedAsync(IDialogContext context, IAwaitable<bool> argument)
        {
            var confirmed = await argument;

            if(confirmed)
            {
                await context.PostAsync("サポートチケットを発行しました。");
            }
            else
            {
                await context.PostAsync("サポートチケットの発行を中止しました。"
                    + "最初からやり直してください。");
            }

            context.Done<object>(null);
        }

        public async Task DescriptionMessageReceviedAsync(IDialogContext context, IAwaitable<string> argument)
        {
            this.description = await argument;
            await context.PostAsync($"承知しました。内容は \"{this.description}\"ですね。");
            context.Done<object>(null);
        }

    }
}