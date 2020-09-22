using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Threading;

namespace ChatClient
{
    public class ChatForm : ModelBase
    {
	    private string _username;
	    private string _message;
		private bool _toggle = true;
		public string ReadData { get; set; }
	    private ObservableCollection<string> _chat;
        public RelayCommand SendCommand { get; set; }
        public RelayCommand ConnectCommand { get; set; }

	    public string Username
	    {
		    get
		    {
			    return _username;
		    }
		    set
		    {
			    _username = value;
                OnPropertyChanged(nameof(Username));
		    }
	    }

	    public string Message
	    {
		    get
		    {
			    return _message;
		    }
		    set
		    {
			    _message = value;
                OnPropertyChanged(nameof(Message));
		    }
	    }

	    public ObservableCollection<string> Chat
	    {
		    get
		    {
			    return _chat;
		    }
		    set
		    {
			    _chat = value;
                OnPropertyChanged(nameof(Chat));
		    }
	    }

		public ChatForm()
		{
			SendCommand = new RelayCommand(SendMessage, CheckMessage);
			ConnectCommand = new RelayCommand(Connect, CheckUser);
			Chat = new ObservableCollection<string>();
	    }

	    private void SendMessage(object obj)
	    {
		    byte[] outStream = System.Text.Encoding.UTF8.GetBytes(Message + "$");
		    serverStream.Write(outStream, 0, outStream.Length);
		    serverStream.Flush();
			Message = "";
		}

	    private bool CheckMessage(object obj)
	    {
		    return (!String.IsNullOrEmpty(Message) && !_toggle);
		}

	    private void Connect(object obj)
	    {
		    ReadData = "Connected to Chat Server ...";
		    msg();

			clientSocket.Connect("10.0.16.215", 8888);
			serverStream = clientSocket.GetStream();
			_toggle = false;



		    byte[] outStream = System.Text.Encoding.UTF8.GetBytes(Username + "$");
		    serverStream.Write(outStream, 0, outStream.Length);
		    serverStream.Flush();

		    Thread ctThread = new Thread(getMessage);
		    ctThread.Start();
		}

	    private bool CheckUser(object obj)
	    {
			return (!String.IsNullOrEmpty(Username) && _toggle);
	    }


		System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();
        NetworkStream serverStream = default(NetworkStream);




        private void getMessage()
        {
            while (true)
            {
                serverStream = clientSocket.GetStream();
                int buffSize = 0;
                byte[] inStream = new byte[clientSocket.ReceiveBufferSize];
                buffSize = clientSocket.ReceiveBufferSize;
					serverStream.Read(inStream, 0, buffSize);
                string returndata = System.Text.Encoding.UTF8.GetString(inStream);
                ReadData = "" + returndata;
				msg();
				//Chat.Add(readData);
			}
        }

        private void msg()
        {
			System.Windows.Application.Current.Dispatcher.Invoke(
				(Action)(() => { Chat.Add(ReadData); }));
        }

    }
}