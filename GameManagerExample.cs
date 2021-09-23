using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Example of a game manager that utilises the state machine example
/// </summary>
/// <remarks>
/// For scene loading this script will be on a Game Object within a base scene, and scenes will be additively loaded and unloaded from it
/// This is just an example and will not run in Unity, a more robust additive scene loader async system is needed, this script demonstrates state machines only
/// </remarks>

namespace NB
{   
    public class GameManagerExample : MonoBehaviour
    {
        /// <summary>
        /// Unity SceneManager instance
        /// </summary>
        SceneManager sceneManager;

        #region State Machine Functions
        /// <summary>
        /// The load scene state machine
        /// </summary>
        StateMachine sceneLoadStateMachine;

        /// <summary>
        /// Names of the scene load states
        /// </summary>
        private const string StateSceneLoadIdle = "State Idle";
        private const string StateLoadScene = "State Load Scene";

        /// <summary>
        /// the game play state machine
        /// </summary>
        StateMachine gamePlayStateMachine;

        /// <summary>
        /// Names of the game play states
        /// </summary>
        private const string StateGameIdle = "State Idle";
        private const string StatePlayGame = "Play Game State";
        private const string StatePauseGame = "Pause Game State";

        #endregion State Machine Functions

        #region Loading State Variables

        private AsyncOperation asyncLoad;

        public GameObject loadingScreen;

        private string sceneName;

        #endregion Loading State Variables

        #region Game State Variables

        public GameObject playingMenu;

        public GameObject pauseMenu;

        #endregion Game State Variables

        void Start()
        {
            DontDestroyOnLoad(this.gameObject);

            List<NBState> states = new List<NBState>();

            //All Callbacks set to null means this state never runs functions, perfect for idling a statemachine
            states.Add(new NBState(null, null, null, StateSceneLoadIdle));
            
            states.Add(new NBState(EnterLoadScene, ExecuteLoadScene, ExitLoadScene, StateLoadScene));

            sceneLoadStateMachine = new StateMachine("Scene Load State Machine", states);

            sceneLoadStateMachine.GoToState(StateSceneLoadIdle);

            states.Clear();

            states.Add(new NBState(null, null, null, StateGameIdle));

            states.Add(new NBState(EnterGamePlay, ExecuteGamePlay, ExitGamePlay, StatePlayGame));
            //Any Callback can be left null as well without issue
            states.Add(new NBState(EnterGamePause, null, ExitGamePause, StatePauseGame));

            gamePlayStateMachine = new StateMachine("Game State Machine", states);

            gamePlayStateMachine.GoToState(StateGameIdle);

            states.Clear();
        }

        private void Update()
        {
            sceneLoadStateMachine.StateMachineUpdate();

            gamePlayStateMachine.StateMachineUpdate();
        }

        #region State Machine Functions
        private void EnterLoadScene()
        {
            Debug.Log("Enter loading scene state");

            loadingScreen.SetActive(true);

            LoadNewScene(sceneName);
        }

        private void ExecuteLoadScene()
        {
            Debug.Log("Execute loading scene state");

            if (asyncLoad.isDone)
            {
                StartGamePlay();

                sceneLoadStateMachine.GoToState(StateSceneLoadIdle);
            }
        }

        private void ExitLoadScene()
        {
            Debug.Log("Exit loading scene state");

            loadingScreen.SetActive(false);
        }

        private void EnterGamePlay()
        {
            Debug.Log("Enter play game state");

            playingMenu.SetActive(true);
        }

        private void ExecuteGamePlay()
        {
            Debug.Log("Execute play game state");

            //Control game play state things here
            //Can keep track of score and timer here, for example
        }

        private void ExitGamePlay()
        {
            Debug.Log("Exit play game state");

            playingMenu.SetActive(false);
        }

        private void EnterGamePause()
        {
            Debug.Log("Enter pause game state");

            pauseMenu.SetActive(true);
        }

        private void ExitGamePause()
        {
            Debug.Log("Exit pause game state");

            pauseMenu.SetActive(false);
        }

        #endregion State Machine Functions

        #region Scene Loading Functions

        /// <summary>
        /// Starts a new scene loading as additive to the main scene
        /// </summary>
        /// <param name="sceneName"> The name of the scene to be loaded </param>
        private void LoadNewScene(string sceneName)
        {
            asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }

        #endregion Scene Loading Functions

        #region Game Play Functions
        public void StartGamePlay()
        {
            gamePlayStateMachine.GoToState(StatePlayGame);
        }

        /// <summary>
        /// Called by a button event
        /// </summary>
        public void PauseButtonPressed()
        {
            gamePlayStateMachine.GoToState(StatePauseGame);
        }

        /// <summary>
        /// Called by a button event
        /// </summary>
        public void ContinueButtonPressed()
        {
            gamePlayStateMachine.GoToState(StatePlayGame);
        }

        #endregion Game Play Functions
    }
}
