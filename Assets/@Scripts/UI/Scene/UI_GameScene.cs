using UnityEngine;
using UnityEngine.EventSystems;

public class UI_GameScene : UI_Scene
{
    enum Images
    {
        Portrait,
        SubHeroIcon,
        MoneyIcon,
        AnnouncementBox,
        SkillAIcon,
        SkillBIcon,
    }
    
    enum Buttons
    {
        InventroyButton,
        HeroesListButton,
    }

    enum Texts
    {
        MoneyCountText,
        FpsText,
        AnnounceText,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindImages(typeof(Images));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));

        GetButton((int)Buttons.InventroyButton).gameObject.BindEvent(OnClickInventroyButton);
        GetButton((int)Buttons.HeroesListButton).gameObject.BindEvent(OnClickHeroesListButton);

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
            GetText((int)Texts.FpsText).text = text;

            GetText((int)Texts.MoneyCountText).text = "";



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

        //GetImage((int)Images.SubHeroIcon).sprite = Managers.Resource.Load<Sprite>(Managers.Data.HeroDic[_heroDataId].IconImage);
    }

    void OnClickHeroesListButton(PointerEventData evt)
    {
        Debug.Log("OnClickHeroesListButton");
        UI_HeroesListPopup popup = Managers.UI.ShowPopupUI<UI_HeroesListPopup>();
        popup.SetInfo();
    }

    void OnClickInventroyButton(PointerEventData evt)
    {
        Debug.Log("OnClickInventroyButton");
    }

    public void RefreshMoneyText()
    {
        GetText((int)Texts.MoneyCountText).text = Managers.Game.Money.ToString();
    }
}