using System;

namespace SmartService.Order
{
	/// <summary>
	/// Summary description for CancelReason.
	/// </summary>
	public class CancelReason
	{
		// Fields
		public int CancelReasonID;
		public string Reason;

		// Methods
		public CancelReason()
		{
			this.CancelReasonID = 0;
			this.Reason = null;
		}

		public CancelReason(int reasonID, string reason)
		{
			this.CancelReasonID = reasonID;
			this.Reason = reason;
		}
	}
}
