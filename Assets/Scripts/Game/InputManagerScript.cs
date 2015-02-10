﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InputManagerScript : MonoBehaviour {

    [SerializeField]
    GameManagerScript _gameManager;

    [SerializeField]
    PlayerScript[] _playerScript;

    [SerializeField]
    Camera _gameCamera;

    [SerializeField]
    Collider _groundCollider;

    [SerializeField]
    NetworkView _networkView;

    // [SerializeField]
    // LineRenderer _lineMovement;

    [SerializeField]
    Button _collectCoinsButton;

    [SerializeField]
    Button _addWayPointButton;

    [SerializeField]
    Button _finishTurnButton;

    [SerializeField]
    Button _cancelButton;

    Vector3 clickPoint = Vector3.zero;

    void Start() {
        _collectCoinsButton.onClick.AddListener(() => {
            _networkView.RPC("WantsToCollectCoins", RPCMode.Server, Network.player);
        });
        _addWayPointButton.onClick.AddListener(() => {
            _networkView.RPC("WantsToAddWayPoint", RPCMode.Server, Network.player, clickPoint);
        });
        _finishTurnButton.onClick.AddListener(() => {
            _networkView.RPC("WantsToFinishTurn", RPCMode.Server, Network.player);
        });
        _cancelButton.onClick.AddListener(() => {
            CancelClick();
        });
    }

    // Update is called once per frame
    void Update() {
        if(Network.isClient) {

            if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) {
                _networkView.RPC("WantsToAddWayPoint", RPCMode.Server, Network.player);
                CancelClick();
            }

            if(Input.GetKeyDown(KeyCode.Return)) {
                _networkView.RPC("WantsToFinishTurn", RPCMode.Server, Network.player);
            }

            if(Input.GetKeyDown(KeyCode.A)) {
                _networkView.RPC("WantsToCollectCoins", RPCMode.Server, Network.player);
            }

            // Initialize clickPoint (PC)
            if(Input.GetMouseButtonUp(0)) {
                var ray = _gameCamera.ScreenPointToRay(Input.mousePosition);

                RaycastHit hitInfo;

                if(_groundCollider.Raycast(ray, out hitInfo, float.MaxValue)) {
                    clickPoint = hitInfo.point;
                }
            }

            // Initialize clickPoint (Android)
            if(Input.touchCount > 0) {
                Touch touch = Input.GetTouch(0);

                if(touch.phase == TouchPhase.Ended) {
                    var ray = _gameCamera.ScreenPointToRay(touch.position);

                    RaycastHit hitInfo;

                    if(_groundCollider.Raycast(ray, out hitInfo, float.MaxValue)) {
                        clickPoint = hitInfo.point;
                    }
                }
            }
        }
    }

    void CancelClick() {
        clickPoint = Vector3.zero;
    }
}