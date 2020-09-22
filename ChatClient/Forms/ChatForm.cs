using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace ChatClient
{
    public class ChatForm : ModelBase
    {
	    private string _username;
	    private string _message;
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
		    byte[] outStream = System.Text.Encoding.ASCII.GetBytes(Message + "$");
		    serverStream.Write(outStream, 0, outStream.Length);
		    serverStream.Flush();
		}

	    private bool CheckMessage(object obj)
	    {
		    return !String.IsNullOrEmpty(Message);
		}

	    private void Connect(object obj)
	    {
		    readData = "Conected to Chat Server ...";
		    msg();
		    clientSocket.Connect("127.0.0.1", 8888);
		    serverStream = clientSocket.GetStream();

		    byte[] outStream = System.Text.Encoding.ASCII.GetBytes(Username + "$");
		    serverStream.Write(outStream, 0, outStream.Length);
		    serverStream.Flush();

		    Thread ctThread = new Thread(getMessage);
		    ctThread.Start();
		}

	    private bool CheckUser(object obj)
	    {
		    return !String.IsNullOrEmpty(Username);
	    }


		System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();
        NetworkStream serverStream = default(NetworkStream);
        string readData = null;




        private void getMessage()
        {
            while (true)
            {
                serverStream = clientSocket.GetStream();
                int buffSize = 0;
                byte[] inStream = new byte[10025];
                buffSize = clientSocket.ReceiveBufferSize;
                serverStream.Read(inStream, 0, buffSize);
                string returndata = System.Text.Encoding.ASCII.GetString(inStream);
                readData = "" + returndata;
				msg();
				//Chat.Add(readData);
			}
        }

        private void msg()
        {
	        Chat.Add(readData);
        }

    }
}