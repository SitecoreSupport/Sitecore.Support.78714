namespace Sitecore.Support.Social.Client.Common.Helpers
{
    using Ninject;
    using Sitecore;
    using Sitecore.Analytics;
    using Sitecore.Analytics.Data;
    using Sitecore.Social.Infrastructure;
    using Sitecore.Social.Infrastructure.Logging;
    using System.Globalization;
    using Ninject.Parameters;
    using Sitecore.Social.Configuration;
    using Sitecore.Social.Configuration.Model;

    public static class SharingHelper
    {
        public static void TriggerGoal(string goalName, string itemId, string defaultGoalName)
        {
            bool xdbSettingsTrackingEnabled =
                ExecutingContext.Current.IoC.Get<IConfigurationFactory>(new IParameter[0])
                    .Get<AnalyticsSettingsConfiguration>()
                    .XdbSettingsTrackingEnabled;
            if (Context.Site != null)
            {
                xdbSettingsTrackingEnabled = xdbSettingsTrackingEnabled && Context.Site.Tracking().EnableTracking;
            }
            if (xdbSettingsTrackingEnabled && (Tracker.Current != null))
            {
                if (string.IsNullOrEmpty(goalName))
                {
                    if (string.IsNullOrEmpty(defaultGoalName) && string.IsNullOrEmpty(itemId))
                    {
                        return;
                    }
                    goalName = defaultGoalName;
                }
                PageEventData data1 = new PageEventData(goalName) {DataKey = itemId};
                PageEventData pageData = data1;
                ILogManager manager = ExecutingContext.Current.IoC.Get<ILogManager>(new IParameter[0]);
                object[] args = new object[] {goalName, itemId};
                manager.LogMessage(
                    string.Format(CultureInfo.InvariantCulture,
                        "Start: trigger the '{0}' share button goal for item with ID = {1}.", args), LogLevel.Info,
                    typeof(SharingHelper));
                if (Tracker.Current.Interaction.PreviousPage != null)
                {
                    Tracker.Current.Interaction.PreviousPage.Register(pageData);
                }
                else
                {
                    manager.LogMessage(
                        "Could not trigger a goal: Analytics.Tracker.Current.Interaction.PreviousPage is null",
                        LogLevel.Warn, typeof(SharingHelper));
                }
                manager.LogMessage("Finish: trigger share button goal.", LogLevel.Info, typeof(SharingHelper));
            }
        }
    }
}