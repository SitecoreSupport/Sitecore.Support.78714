namespace Sitecore.Support.Social.Client.Sharing
{
  using Analytics;
  using Analytics.Configuration;
  using Sitecore.Social.Client.Common.Helpers;
  using Sitecore.Social.Encryption;
  using System;
  using System.Web;
  using System.Web.UI;

  public class SocialEvents : Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      if (AnalyticsSettings.Enabled)
      {
        if (Tracker.Current != null && Tracker.Current.CurrentPage != null)
        {
          Tracker.Current.CurrentPage.Cancel();
        }
        SharingHelper.TriggerGoal(GoalName, ItemId, DefaultGoalName);
      }
    }

    protected string DefaultGoalName
    {
      get
      {
        if (!string.IsNullOrEmpty(Request.QueryString["defaultGoalName"]))
        {
          return StringCipher.Decrypt(HttpUtility.UrlDecode(Request.QueryString["defaultGoalName"]));
        }
        return string.Empty;
      }
    }

    protected string GoalName
    {
      get
      {
        if (!string.IsNullOrEmpty(Request.QueryString["goalName"]))
        {
          return StringCipher.Decrypt(HttpUtility.UrlDecode(Request.QueryString["goalName"]));
        }
        return string.Empty;
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
  }
}
