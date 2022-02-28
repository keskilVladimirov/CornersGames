using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Button buttonPvP;
    [SerializeField] private Button buttonPvE;
    [SerializeField] private Button buttonStart;
    
    [SerializeField] private GameObject panelMenu;
    [SerializeField] private GameObject panelRules;
    
    [SerializeField] private Toggle  toggleDiagonalMode ;
    [SerializeField] private Toggle  toggleVerHorMode;
    [SerializeField] private Toggle  togglelRulesNoMode;

    private bool PvP;
    private string gameMode;
    
    private void Start()
    {
        buttonPvP.OnPointerClickAsObservable()
            .Subscribe(_ => ClickButtonPvP()).AddTo(this);
        
        //TODO: AI 
        /*buttonPvE.OnPointerClickAsObservable()
            .Subscribe(_ => ClickButtonPvE()).AddTo(this);*/
        
        buttonStart.OnPointerClickAsObservable()
            .Subscribe(_ => ClickButtonStart()).AddTo(this);
        
        toggleDiagonalMode.OnValueChangedAsObservable()
            .Subscribe(b =>
            {
                if (b) SetGameMode(0);
            }).AddTo(this);
        
        toggleVerHorMode.OnValueChangedAsObservable()
            .Subscribe(b=> {
                if (b) SetGameMode(1);
            }).AddTo(this);
        
        togglelRulesNoMode.OnValueChangedAsObservable()
            .Subscribe(b => {
                if (b) SetGameMode(2);
            }).AddTo(this);
    }
    
    private void ClickButtonPvP()
    {
        SetPanel();
        PvP = true;
    }
    
    private void ClickButtonPvE()
    {
        SetPanel();
        PvP = false;
    }
    
    private void ClickButtonStart()
    {
        var index = PvP == true ? 0 : 1;
        PlayerPrefs.SetInt("pvp", index);
        PlayerPrefs.SetString("gameMode",gameMode);
        
        SceneManager.LoadScene(1);
    }

    private void SetGameMode(int index)
    {
        switch (index)
        {
            case 0:
                gameMode = "DiagonalMode";
                break;
            case 1:
                gameMode = "VerHorMode";
                break;
            case 2:
                gameMode = "NoMode";
                break;
        }
    }
    
    private void SetPanel()
    {
        panelMenu.SetActive(false);
        panelRules.SetActive(true);
    }
}
