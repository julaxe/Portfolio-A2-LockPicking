using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkill : MonoBehaviour
{
   private Scrollbar _scrollbar;

   public float skill = 0.0f;

   private void Awake()
   {
      _scrollbar = GetComponentInChildren<Scrollbar>();
   }

   private void Update()
   {
      skill = _scrollbar.value;
   }
}
