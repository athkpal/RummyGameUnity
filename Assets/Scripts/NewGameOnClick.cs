﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameOnClick : MonoBehaviour
{
    public void LoadOnClick(int sceneIndex)
    {
        SceneManager.LoadSceneAsync(sceneIndex);
    }
}
