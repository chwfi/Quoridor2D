using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputReader _input;

    [SerializeField] private Tilemap _groundTilemap;
    [SerializeField] private Tilemap _obstacleTilemap;

    private void OnEnable()
    {
        if (_input != null)
        {
            var playerInput = new Controls();
            playerInput.Player.SetCallbacks(_input);
            playerInput.Player.Enable();
        }
    }

    private void Update()
    {
        if (_input.MoveInput.magnitude > 0 && CanMove(_input.MoveInput))
        {
            transform.position += (Vector3)_input.MoveInput;
            _input.StopMove();
        }
    }

    private bool CanMove(Vector2 dir)
    {
        Vector3Int gridPos = _groundTilemap.WorldToCell(transform.position + (Vector3)dir);
        if (!_groundTilemap.HasTile(gridPos))
        { 
            return false;
        }
        else return true;
    }
}
