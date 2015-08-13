using System;

// 1. Download and install Microsoft Office Communicator 2007 SDK
//    http://www.microsoft.com/downloads/en/details.aspx?FamilyID=ed1cce45-cc22-46e1-bd50-660fe6d2c98c&displaylang=en
// 2. References -> Add Reference -> Browse
//    "C:\Program Files\Microsoft Office Communicator\SDK\CommunicatorAPI.dll"
//    "C:\Program Files\Microsoft Office Communicator\SDK\CommunicatorPrivate.dll"
using CommunicatorAPI;
using CommunicatorPrivate;

namespace Vurdalakov.MorseMc
{
    public class MessengerContact
    {
        private CommunicatorAPI.Messenger messenger;
        private IMessengerContactAdvanced contact;

        public MessengerContact()
        {
            messenger = new CommunicatorAPI.Messenger();

            GetContact(messenger.MySigninName, messenger.MyServiceId);
        }

        public MessengerContact(String singinName, String serviceId)
        {
            messenger = new CommunicatorAPI.Messenger();

            GetContact(singinName, serviceId);
        }

        private void GetContact(String singinName, String serviceId)
        {
            contact = messenger.GetContact(singinName, serviceId) as IMessengerContactAdvanced;

            messenger.OnContactStatusChange += new DMessengerEvents_OnContactStatusChangeEventHandler(OnContactStatusChange);
        }

        public MessengerContactAvailability Availability
        {
            get
            {
                Object[] presenceProperties = contact.PresenceProperties as Object[];

                return (MessengerContactAvailability)presenceProperties[(int)PRESENCE_PROPERTY.PRESENCE_PROP_AVAILABILITY];
            }
            
            set
            {
                Object[] presenceProperties = contact.PresenceProperties as Object[];

                presenceProperties = new Object[presenceProperties.Length];

                presenceProperties[(int)PRESENCE_PROPERTY.PRESENCE_PROP_AVAILABILITY] = value;

                contact.PresenceProperties = presenceProperties as Object;
            }
        }

        /// <summary>
        /// Retrieves the friendly name of the contact.
        /// </summary>
        public String FriendlyName
        {
            get { return contact.FriendlyName; }
        }

        void OnContactStatusChange(Object eventContactObject, MISTATUS eventStatus)
        {
            IMessengerContact eventContact = eventContactObject as IMessengerContact;
            if ((null == eventContact) || (eventContact.SigninName != contact.SigninName))
            {
                return;
            }

            AvailabilityChanged(this, new EventArgs());
        }

        public delegate void AvailabilityChangedEventHandler(object sender, EventArgs e);

        public event AvailabilityChangedEventHandler AvailabilityChanged;
    }
}
