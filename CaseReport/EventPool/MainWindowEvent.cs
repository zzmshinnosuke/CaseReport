using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseReport.EventPool
{
    public enum MainMessageType
    {
        UpdateCase,
    }

    public class MainWindowEvent
    {
        private static MainWindowEvent m_Instance = new MainWindowEvent();
        public static MainWindowEvent GetInstance()
        {
            return m_Instance;
        }

        public delegate void MainResponseHandler(MainMessageType msgType);
        public event MainResponseHandler MainResponseEvent;
        public void MainResponse(MainMessageType msgType)
        {
            if (MainResponseEvent != null)
            {
                MainResponseEvent(msgType);
            }
        }
    }
}
