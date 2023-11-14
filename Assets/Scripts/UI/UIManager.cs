using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Canvas completePanel;       

        private void Start()
        {
            if (Piston.instanceExists)
            {
                Piston.instance.allPartsAssembled += OnAllPartsAssembled;
            }
        }

        private void OnDestroy()
        {
            if (Piston.instanceExists)
            {
                Piston.instance.allPartsAssembled -= OnAllPartsAssembled;
            }
        }

        private void OnAllPartsAssembled()
        {
            completePanel.enabled = true;
        }

        public void OnTappedRestart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}