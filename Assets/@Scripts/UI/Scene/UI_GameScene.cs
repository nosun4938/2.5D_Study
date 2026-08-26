using UnityEngine;
using UnityEngine.EventSystems;

public class UI_GameScene : UI_Scene
{
    enum Buttons
    {
        MoneyPlusButton,
        HeroesListButton,
        SetHeroesButton,
        SettingButton,
        InventoryButton,
        WorldMapButton,
        QuestButton,
        PortalButton,
        CampButton,
        CheatButton,
    }

    enum Texts
    {
        LevelText,
        MoneyCountText,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));

        GetButton((int)Buttons.MoneyPlusButton).gameObject.BindEvent(OnClickGoldPlusButton);
        GetButton((int)Buttons.HeroesListButton).gameObject.BindEvent(OnClickHeroesListButton);
        GetButton((int)Buttons.SetHeroesButton).gameObject.BindEvent(OnClickSetHeroesButton);
        GetButton((int)Buttons.SettingButton).gameObject.BindEvent(OnClickSettingButton);
        GetButton((int)Buttons.InventoryButton).gameObject.BindEvent(OnClickInventoryButton);
        GetButton((int)Buttons.WorldMapButton).gameObject.BindEvent(OnClickWorldMapButton);
        GetButton((int)Buttons.QuestButton).gameObject.BindEvent(OnClickQuestButton);
        GetButton((int)Buttons.PortalButton).gameObject.BindEvent(OnClickPortalButton);
        GetButton((int)Buttons.CampButton).gameObject.BindEvent(OnClickCampButton);
        GetButton((int)Buttons.CheatButton).gameObject.BindEvent(OnClickCheatButton);

        Refresh();

        return true;
    }

    private float _elapsedTime = 0.0f;
    private float _updateInterval = 1.0f;

    private void Update()
    {
        _elapsedTime += Time.deltaTime;

        if (_elapsedTime >= _updateInterval)
        {
            float fps = 1.0f / Time.deltaTime;
            float ms = Time.deltaTime * 1000.0f;
            string text = string.Format("{0:N1} FPS ({1:N1}ms)", fps, ms);
            GetText((int)Texts.MoneyCountText).text = text;

            _elapsedTime = 0;
        }
    }

    public void SetInfo()
    {
        Refresh();
    }

    void Refresh()
    {
        if (_init == false)
            return;
    }

    void OnClickGoldPlusButton(PointerEventData evt)
    {
        Debug.Log("OnOnClickGoldPlusButton");
    }

    void OnClickHeroesListButton(PointerEventData evt)
    {
        Debug.Log("OnClickHeroesListButton");
        UI_HeroesListPopup popup = Managers.UI.ShowPopupUI<UI_HeroesListPopup>();
        popup.SetInfo();
    }

    void OnClickSetHeroesButton(PointerEventData evt)
    {
        Debug.Log("OnClickSetHeroesButton");
    }

    void OnClickSettingButton(PointerEventData evt)
    {
        Debug.Log("OnClickSettingButton");
    }

    void OnClickInventoryButton(PointerEventData evt)
    {
        Debug.Log("OnClickInventoryButton");
    }

    void OnClickWorldMapButton(PointerEventData evt)
    {
        Debug.Log("OnClickWorldMapButton");
    }

    void OnClickQuestButton(PointerEventData evt)
    {
        Debug.Log("OnClickQuestButton");
    }

    void OnClickCampButton(PointerEventData evt)
    {
        Debug.Log("OnClickCampButton");
    }

    void OnClickPortalButton(PointerEventData evt)
    {
        Debug.Log("OnClickPortalButton");
    }

    void OnClickCheatButton(PointerEventData evt)
    {
        Debug.Log("OnClickCheatButton");
    }

    public void RefreshMoneyText()
    {
        GetText((int)Texts.MoneyCountText).text = Managers.Game.Money.ToString();
    }
}