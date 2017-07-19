namespace Sitecore.Support.Social.Client.Sharing
{
  using Ninject;
  using Ninject.Parameters;
  using Sitecore;
  using Sitecore.Analytics;
  using Sitecore.Social.Client.Common.Helpers;
  using Sitecore.Social.Configuration;
  using Sitecore.Social.Configuration.Model;
  using Sitecore.Social.Encryption;
  using Sitecore.Social.Infrastructure;
  using System;
  using System.Web;
  using System.Web.UI;

  public class SocialEvents : Page
  {
    #region Modified code

    protected void Page_Load(object sender, EventArgs e)
    {
      bool xdbSettingsTrackingEnabled = ExecutingContext.Current.IoC.Get<IConfigurationFactory>(new IParameter[0]).Get<AnalyticsSettingsConfiguration>().XdbSettingsTrackingEnabled;

      if (Sitecore.Context.Site != null)
      {
        xdbSettingsTrackingEnabled = xdbSettingsTrackingEnabled && Sitecore.Context.Site.Tracking().EnableTracking;
      }

      if (xdbSettingsTrackingEnabled && (Tracker.Current != null) && (Tracker.Current.CurrentPage != null))
      {
        Tracker.Current.CurrentPage.Cancel();
      }

      SharingHelper.TriggerGoal(this.GoalName, this.ItemId, this.DefaultGoalName);
    }

    #endregion

    #region Original code

    protected string DefaultGoalName
    {
      get
      {
        if (string.IsNullOrEmpty(base.Request.QueryString["defaultGoalName"]))
        {
          return string.Empty;
        }

        return StringCipher.Decrypt(HttpUtility.UrlDecode(base.Request.QueryString["defaultGoalName"]));
      }
    }

    protected string GoalName
    {
      get
      {
        if (string.IsNullOrEmpty(base.Request.QueryString["goalName"]))
        {
          return string.Empty;
        }

        return StringCipher.Decrypt(HttpUtility.UrlDecode(base.Request.QueryString["goalName"]));
      }
    }

    protected string ItemId
    {
      get
      {
        if (Request.QueryString["itemId"] != null)
        {
          return Request.QueryString["itemId"];
        }

        return string.Empty;
      }
    }

    #endregion

  }
}
