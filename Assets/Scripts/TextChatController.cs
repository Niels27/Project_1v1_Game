using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Mirror;
using UnityEngine.UI;


public class TextChatController : NetworkBehaviour
{
    public Canvas Canvas;

    public TMP_Text ChatMessageText;

    public bool InUse;

    private TMP_InputField _inputField;
    private Transform _scrollViewContent;

    public class ChatMessage : MessageBase
    {
        public string Name;
        public string TimeStamp;
        public string Message;
    }

    public void Start()
    {
        NetworkClient.RegisterHandler<ChatMessage>(OnChatMessage);

        _inputField = Canvas.transform.Find("MsgInputField").GetComponent<TMP_InputField>();
        _scrollViewContent = Canvas.transform.Find("MsgScrollView/Viewport/Content");
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Return) && InUse)
        {
            SendChatMessage(_inputField.text);
        }
    }

    public void OnInputFieldSelect()
    {
        InUse = true;
    }

    public void OnInputFieldDeselect()
    {
        InUse = false;
    }

    public void OnSendButtonClick()
    {
        SendChatMessage(_inputField.text);
    }

    public void OnChatMessage(NetworkConnection conn, ChatMessage msg)
    {
        var newChatMsgText = Instantiate(ChatMessageText, _scrollViewContent.transform, false);
        newChatMsgText.text = $"[{msg.TimeStamp}][{msg.Name}] {msg.Message}";

        print(msg.Message);
    }

    public void SendChatMessage(string message)
    {
        if (message.Length == 0) return;

        ChatMessage msg = new ChatMessage()
        {
            Name = "Player",
            TimeStamp = DateTime.Now.ToString("HH:mm:ss"),
            Message = message
        };

        NetworkClient.Send(msg);

        _inputField.text = "";
    }
}
