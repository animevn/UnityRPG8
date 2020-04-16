using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

namespace Script.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        private enum DestinationIdentifier
        {
            A, B, C
        }
        
        [SerializeField] private int sceneToLoad = -1;
        [SerializeField] private Transform spawnPawn;
        [SerializeField] private DestinationIdentifier destination;
        [SerializeField] private float fadeWaitTime = 2f;
        [SerializeField] private float timeFadeOut = 1f;
        [SerializeField] private float timeFadeIn = 2f;

        private static bool IsPlayerCollider(Component player)
        {
            return player.CompareTag("Player");
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (IsPlayerCollider(other))
            {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            if (sceneToLoad < 0)    
            {
                Debug.LogError("No scene to load");
                yield break;
            }
            var fader = FindObjectOfType<Fader>();
            DontDestroyOnLoad(gameObject);
            yield return fader.FadeOut(timeFadeOut);
            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            UpdatePlayer(GetOtherPortal());
            yield return new WaitForSeconds(fadeWaitTime);
            yield return fader.FadeIn(timeFadeIn);
            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            var player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPawn.position);
            player.transform.rotation = otherPortal.spawnPawn.rotation;
            
        }

        private Portal GetOtherPortal()
        {
            foreach (var portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) continue;
                if (portal.destination != destination) continue;
                return portal;
            }

            return null;
        }
    }
}
