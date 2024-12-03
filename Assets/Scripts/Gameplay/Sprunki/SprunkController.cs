using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprunkController : MonoBehaviour
{
    [Header("======= Character =======")]
    [SerializeField] private List<CharacterController> _characters;
    
    [Header("======= Beat =======")]
    [SerializeField] private List<BeatController> _beats;
}
