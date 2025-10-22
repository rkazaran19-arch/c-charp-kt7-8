using System;

namespace App.Topics.ActionDelegates.T2_ActionPipeline
{
    public class ActionPipeline
    {
        public static int InvokeAll(string input, params Action<string>[] handlers)
        {
            if (handlers == null)
                throw new ArgumentNullException(nameof(handlers));

            int successCount = 0;

            foreach (var handler in handlers)
            {
                if (handler == null)
                    continue;

                try
                {
                    handler(input);
                    successCount++;
                }
                catch
                {
                    throw;
                }
            }

            return successCount;
        }
    }
}