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

        /// <summary>
        /// The game state machine
        /// </summary>
        StateMachine gameStateMachine = new StateMachine();

        #region Loading State Variables

        private AsyncOperation asyncLoad;

        public GameObject loadingScreen;

        #endregion Loading State Variables

        void Start()
        {
            DontDestroyOnLoad(this.gameObject);

            gameStateMachine.ChangeState(new LoadSceneState(this, "Scene1"));
        }

        private void Update()
        {
            gameStateMachine.Update();
        }

        #region Loading State Functions
        public void ShowLoadingScreen()
        {
            loadingScreen.SetActive(true);
        }

        public void LoadNewScene(string sceneName)
        {
            asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }

        public bool CheckSceneLoaded()
        {
            return asyncLoad.isDone;
        }

        public void HideLoadingScreen()
        {
            loadingScreen.SetActive(false);
        }

        #endregion Loading State Functions

        #region Run Scene State Functions
        public void StartGamePlay()
        {
            gameStateMachine.ChangeState(new RunSceneState(this));
        }

        #endregion Run Scene State Functions
    }

    /// <summary>
    /// Example script for loading a scene through a state machine
    /// </summary>
    public class LoadSceneState : NBIState
    {
        GameManagerExample owner;

        string sceneName;

        public LoadSceneState(GameManagerExample owner, string sceneName)
        {
            this.owner = owner;

            this.sceneName = sceneName;
        }

        public void Enter()
        {
            Debug.Log("Enter loading scene state");

            owner.ShowLoadingScreen();

            owner.LoadNewScene(sceneName);
        }

        public void Execute()
        {
            Debug.Log("Execute loading scene state");

            if (owner.CheckSceneLoaded())
            {
                owner.StartGamePlay();
            }
        }

        public void Exit()
        {
            Debug.Log("Exit loading scene state");

            owner.HideLoadingScreen();
        }
    }

    /// <summary>
    /// Example script for running scene gameplay through a state machine
    /// </summary>
    public class RunSceneState : NBIState
    {
        GameManagerExample owner;

        public RunSceneState(GameManagerExample owner)
        {
            this.owner = owner;
        }

        public void Enter()
        {
            Debug.Log("Enter run scene state");
        }

        public void Execute()
        {
            Debug.Log("Execute run scene state");
        }

        public void Exit()
        {
            Debug.Log("Exit run scene state");
        }
    }
}
