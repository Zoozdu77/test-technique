using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HutScript : MonoBehaviour
{
    public List<Transform> cubes = new();
    [SerializeField] private GameObject menu;
    [SerializeField] private TextMeshProUGUI crateNumber;
    [SerializeField] private LayerMask UILayer;
    [HideInInspector] public Transform player;

    private void Update()
    {
        crateNumber.text = cubes.Count.ToString();
    }

    public void OpenMenu(Transform Player)
    {
        menu.SetActive(true);
        player = Player;
    }
    
    public void CloseMenu()
    {
        menu.SetActive(false);
        player.GetComponent<PlayerController>().hutteOuverte = false;
    }

    public void TakeACrate()
    {
        if (player != null && cubes.Count != 0)
        {
            player.GetComponent<PlayerController>().cubesOnPlayer.Add(cubes[^1]);
            cubes[^1].parent = player.GetChild(player.GetComponent<PlayerController>().cubesOnPlayer.Count);
            cubes[^1].localPosition = new(0,0,0);
            cubes.Remove(cubes[^1]);
        }
    }

    public void LeaveACrate()
    {
        if(player != null && player.GetComponent<PlayerController>().cubesOnPlayer.Count != 0)
        {
            player.GetComponent<PlayerController>().cubesOnPlayer[^1].parent = transform;
            player.GetComponent<PlayerController>().cubesOnPlayer[^1].position = transform.position;
            cubes.Add(player.GetComponent<PlayerController>().cubesOnPlayer[^1]);
            player.GetComponent<PlayerController>().cubesOnPlayer.Remove(player.GetComponent<PlayerController>().cubesOnPlayer[^1]);
        }
    }
}
