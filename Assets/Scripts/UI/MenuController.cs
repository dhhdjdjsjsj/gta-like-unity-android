using UnityEngine;

namespace GtaLike.UI
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private GameObject settingsMenu;
        [SerializeField] private GameObject friendsMenu;

        public void PlaySingle()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
        }

        public void ShowSettings()
        {
            SetActiveMenu(settingsMenu);
        }

        public void ShowFriends()
        {
            SetActiveMenu(friendsMenu);
        }

        public void BackToMain()
        {
            SetActiveMenu(mainMenu);
        }

        public void Quit()
        {
            Application.Quit();
        }

        private void SetActiveMenu(GameObject target)
        {
            if (mainMenu != null) mainMenu.SetActive(target == mainMenu);
            if (settingsMenu != null) settingsMenu.SetActive(target == settingsMenu);
            if (friendsMenu != null) friendsMenu.SetActive(target == friendsMenu);
        }
    }
}
