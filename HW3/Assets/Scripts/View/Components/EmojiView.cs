using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class EmojiView : MonoBehaviour, IPunObservable
{
    [SerializeField] private PhotonView photonView;
    [SerializeField] private GameObject src;
    [SerializeField] private Sprite[] emoji = new Sprite[4];
    [SerializeField] private float timeVisible = 3f;

    private Image _image;
    private int _indexEmoji;
    private float _currentTime;

    // Start is called before the first frame update
    void Start()
    {
        _image = src.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
            return;

        if (src.activeSelf)
        {
            _currentTime += Time.deltaTime;
            if (_currentTime > timeVisible)
            {
                src.SetActive(false);
            }
        }

        if (Input.GetKey(KeyCode.Alpha1))
        {
            _indexEmoji= 0;
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            _indexEmoji= 1;
        }
        else if (Input.GetKey(KeyCode.Alpha3))
        {
            _indexEmoji= 2;
        }
        else if (Input.GetKey(KeyCode.Alpha4))
        {
            _indexEmoji= 3;
        }
        else return;

        _image.sprite = emoji[_indexEmoji];
        src.SetActive(true);
        _currentTime = 0f;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(src.activeSelf);
            stream.SendNext(_indexEmoji);
        }
        else
        {
            src.SetActive((bool) stream.ReceiveNext());
            _indexEmoji = (int) stream.ReceiveNext();
            _image.sprite = emoji[_indexEmoji];
        }
    }
}