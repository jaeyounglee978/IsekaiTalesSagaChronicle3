using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// 필드의 유닛을 전부 모아서 속도로 정렬해서 큐에 밀어넣기
// 그 다음부터는 어쨋든 그거 알지? 알잘딱깔센
// 가장 먼저 행동할 유닛 선택
// 행동 선택
// 행동 이후, 턴 종료. 다음 행동할 유닛 선택
// 다음 행동할 유닛이 플레이어 유닛이면 -> 입력 대기
// 다음 행동할 유닛이 NPC 면 알아서 입력해둔 행동패턴 수행
public class Battle : MonoBehaviour
{
    public GameObject commandButtonPanel;
    public Text panelText;
    public UIButton attackButton, skillButton, guardButton, escapeButton;
    public GameObject test;
    public GameObject playerUnitSection;
    public GameObject enemyUnitSection;
    private int battleTotalSkillCost;
    public Text skillCostText;
    private string skillCostTextFormat = "SkillCost: {0}/10";
    private UIButton currentSelectedButton = null;

    // 유닛들
    private List<Unit> totalUnitList;
    private List<Unit> enemyUnitList;

    private Unit currentActiveUnit;
    private Unit currentSelectedTarget;
    private List<Unit> currentSelectedTargetList = null;

    // 전투 flow 분기용 변수들
    private bool waitUserInput = false;
    private bool waitUserTargetSelection = false;
    private bool isTargetSingle = false;
    private bool isUsingSkill = true;
    private TargetSelectionType selectFromType = TargetSelectionType.DEFAULT;


    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        userSelectionLogic();
        doMonsterLogic();
    }

    private void Initialize()
    {
        // 배틀 시작 시에 init 하기
        totalUnitList = new List<Unit>();
        InitiateEnemyUnit();
        InitiatePlayerUnit();
        totalUnitList = totalUnitList.OrderBy(u => u.speed).ToList(); ;

        // TODO : 전투준비 띄우기
        // TODO : 배틀 시작 로고 보여주기
        battleTotalSkillCost = BattleGlobal.skillCost;
        skillCostText.text = string.Format(skillCostTextFormat, battleTotalSkillCost);
        initiateButtonOnClickEvent();
        if (BattleGlobal.userUnitList.Contains(currentActiveUnit))
        {
            waitUserInput = true;
        }

        Debug.Log("Initiate End");
    }

    private void InitiateEnemyUnit()
    {
        // TODO : 몬스터 정보 저장한 패턴 읽어서 파싱하고 세팅하기
        // TODO : 캐릭터들 배치할 것.
        var fileName = BattleGlobal.monsterPatternFilePath;
        // List<Unit> enemyUnitList = new List<Unit>();
        var enemyUnitList = new List<Unit>
        {
            new Enemy(
                GameObject.Instantiate(test),
                new UnitData(){
                    name = "enemy 1",
                    speed = 14,
                    attack = 10,
                    isInGuardPosition = false,
                    skillCost = 1,
                    targetRangeType = TargetRangeType.SINGLE,
                    skillTargetType = TargetSelectionType.ENEMY,
                    maxHealth = 25,
                    initHealth = 25
                },
                ul => Debug.Log("뿅1"),
                (pul, eul) => Debug.Log("뿅1")
            ),
            new Enemy(
                GameObject.Instantiate(test),
                new UnitData(){
                    name = "enemy 2",
                    speed = 13,
                    attack = 10,
                    isInGuardPosition = false,
                    skillCost = 2,
                    targetRangeType = TargetRangeType.ALL,
                    skillTargetType = TargetSelectionType.ENEMY,
                    maxHealth = 25,
                    initHealth = 25
                },
                ul => Debug.Log("뿅2"),
                (pul, eul) => Debug.Log("뿅2")
            )
        };

        var enemyUnitCount = 1;

        enemyUnitList.ForEach(eu =>
        {
            eu.gameObject.transform.SetParent(enemyUnitSection.transform);
            eu.gameObject.transform.localPosition = new Vector2(0, 300 - 120 * enemyUnitCount);
            eu.gameObject.transform.localScale = new Vector2(0.5f, 0.5f);
            enemyUnitCount += 1;
        });
        this.enemyUnitList = enemyUnitList;
        totalUnitList.AddRange(enemyUnitList);
    }

    private void InitiatePlayerUnit()
    {
        // test code
        if (BattleGlobal.userUnitList == null || BattleGlobal.userUnitList.Count == 0)
        {
            BattleGlobal.userUnitList = new List<Unit>
            {
                new PlayerUnit(
                    GameObject.Instantiate(test),
                    new UnitData(){
                        name = "unit 1",
                        speed = 14,
                        attack = 10,
                        isInGuardPosition = false,
                        skillCost = 1,
                        targetRangeType = TargetRangeType.SINGLE,
                        skillTargetType = TargetSelectionType.ENEMY,
                        maxHealth = 100,
                        initHealth = 100
                    },
                    ul => {
                        if (ul.Count != 0)
                        {
                            Debug.LogFormat("{0} skill to {1}", this.name, ul.First().name);
                            ul.First().GetDamage(15);
                        }
                    }
                ),
                new PlayerUnit(
                    GameObject.Instantiate(test),
                    new UnitData(){
                        name = "unit 2",
                        speed = 13,
                        attack = 10,
                        isInGuardPosition = false,
                        skillCost = 2,
                        targetRangeType = TargetRangeType.ALL,
                        skillTargetType = TargetSelectionType.ENEMY,
                        maxHealth = 100,
                        initHealth = 100
                    },
                    ul => {
                        if (ul.Count != 0)
                        {
                            Debug.LogFormat("{0} skill to {1}", this.name, String.Join(", ", ul.Select(u => u.name)));
                            ul.ForEach(tu => tu.GetDamage(5));
                        }
                    }
                ),
                new PlayerUnit(
                    GameObject.Instantiate(test),
                    new UnitData(){
                        name = "unit 3",
                        speed = 12,
                        attack = 10,
                        isInGuardPosition = false,
                        skillCost = 3,
                        targetRangeType = TargetRangeType.SINGLE,
                        skillTargetType = TargetSelectionType.ALLY,
                        maxHealth = 100,
                        initHealth = 100
                    },
                    ul => {
                        if (ul.Count != 0)
                        {
                            Debug.LogFormat("{0} skill to {1}", this.name, ul.First().name);
                            ul.First().GetDamage(-15);
                        }
                    }
                ),
                new PlayerUnit(
                    GameObject.Instantiate(test),
                    new UnitData(){
                        name = "unit 4",
                        speed = 11,
                        attack = 10,
                        isInGuardPosition = false,
                        skillCost = 4,
                        targetRangeType = TargetRangeType.ALL,
                        skillTargetType = TargetSelectionType.ALLY,
                        maxHealth = 100,
                        initHealth = 100
                    },
                    ul => {
                        if (ul.Count != 0)
                        {
                            Debug.LogFormat("skill to {0}", ul.First().name);
                            ul.ForEach(tu => tu.GetDamage(-5));
                        }
                    }
                )
            };
        }
        // 유닛 소환
        // 및 queue 세팅
        // 및 현재 액티브 유닛 세팅.
        // TODO : 캐릭터들 배치할 것.
        var playerUnitCount = 1;
        BattleGlobal.userUnitList.ForEach(pu =>
        {
            pu.gameObject.transform.SetParent(playerUnitSection.transform);
            pu.gameObject.transform.localPosition = new Vector2(0, 300 - 120 * playerUnitCount);
            pu.gameObject.transform.localScale = new Vector2(0.5f, 0.5f);
            playerUnitCount += 1;
        });

        totalUnitList.AddRange(BattleGlobal.userUnitList);
        changeCurrentActiveUnit();
    }

    private void TurnEnd()
    {
        CalculateUnitHealth();
        unselectButton();
        currentSelectedButton = null;

        if (enemyUnitList.Count == 0)
        {
            Debug.Log("뽜밤뽜밤~ 적을 처치했습니다~");
            BattleEnd();
        }
        else if (BattleGlobal.userUnitList.Count == 0)
        {
            Debug.Log("당신은 패배했습니다!!!!!!");
        }
        else
        {
            changeCurrentActiveUnit();
            if (BattleGlobal.userUnitList.Contains(currentActiveUnit))
            {
                waitUserInput = true;
            }
            currentSelectedTargetList = null;
            currentSelectedTarget = null;
            if (battleTotalSkillCost < 10) battleTotalSkillCost += 1;
            skillCostText.text = string.Format(skillCostTextFormat, battleTotalSkillCost);
        }
    }

    private void CalculateUnitHealth()
    {
        var tempList = new List<Unit>(totalUnitList);
        tempList.ForEach(u =>
        {
            if (u.health <= 0)
            {
                Destroy(u.gameObject);

                totalUnitList.Remove(u);

                if (enemyUnitList.Contains(u))
                {
                    enemyUnitList.Remove(u);
                }
                else if (BattleGlobal.userUnitList.Contains(u))
                {
                    BattleGlobal.userUnitList.Remove(u);
                }
            }
        });
    }

    private void BattleEnd()
    {
        BattleGlobal.skillCost = battleTotalSkillCost;
        SceneManager.LoadScene("Field", LoadSceneMode.Single);
    }

    // 공격 : 유닛 턴제
    // 유닛의 스피드에 따라서 보여주기
    public void SelectAttack()
    {
        waitUserInput = true;
        waitUserTargetSelection = true;
        selectFromType = TargetSelectionType.ENEMY;
        isTargetSingle = true;
        isUsingSkill = false;
        turnToTargetSelectUI();
    }

    public void SelectSkill()
    {
        var unitSkillCost = currentActiveUnit.skillCost;

        if (unitSkillCost > battleTotalSkillCost)
        {
            Debug.LogFormat("코스트가 부족합니다!");
        }
        else
        {
            Debug.LogFormat("스킬 발사 가능");
            waitUserInput = true;
            waitUserTargetSelection = true;
            selectFromType = currentActiveUnit.skillTargetType;
            isTargetSingle = (currentActiveUnit.targetRangeType == TargetRangeType.SINGLE);
            isUsingSkill = true;
            turnToTargetSelectUI();
        }
    }

    public void DoAttack(Unit selectedUnit, Unit targetUnit)
    {
        waitUserInput = false;

        BattleCalculator.CalculateAttack(selectedUnit, targetUnit);

        TurnEnd();
    }

    public void UseSkill(Unit selectedUnit, List<Unit> targetUnits)
    {
        waitUserInput = false;
        selectFromType = TargetSelectionType.DEFAULT;

        this.battleTotalSkillCost -= selectedUnit.skillCost;
        selectedUnit.UseSkill(targetUnits);

        TurnEnd();
    }

    public void DoGuard()
    {
        waitUserInput = false;

        Debug.LogFormat("{0}가 방어태세에 들어갔습니다.", currentActiveUnit.name);

        TurnEnd();
    }

    public void TryEscape()
    {
        waitUserInput = false;
        // 도망 가능 확률 계산하기.
        var isSuccess = UnityEngine.Random.Range(0, 100);

        if (isSuccess < 70)
        {
            Debug.Log("감당하십시오");
            waitUserInput = true;
        }
        else
        {
            Debug.Log("도망쳤습니다");
            // 씬 로드
        }
    }

    private void turnToTargetSelectUI()
    {
        // TODO: 선택 페이즈로 이행
        commandButtonPanel.SetActive(false);
        panelText.text = "대상을 선택하십시오.";
    }

    private void turnBackToCommandUI()
    {
        waitUserTargetSelection = false;
        selectFromType = TargetSelectionType.DEFAULT;
        panelText.text = "무엇을 할까?";
        commandButtonPanel.SetActive(true);
    }

    private void turnToBattleUI()
    {

    }

    private void changeCurrentActiveUnit()
    {
        if (currentActiveUnit != null)
        {
            currentActiveUnit.outline.effectDistance = new Vector2(1, 1);
            totalUnitList.Add(currentActiveUnit);
        }
        currentActiveUnit = totalUnitList.First();
        totalUnitList.RemoveAt(0);
        currentActiveUnit.outline.effectDistance = new Vector2(15, 15);

        if (BattleGlobal.userUnitList.Contains(currentActiveUnit))
        {
            commandButtonPanel.SetActive(true);
            waitUserInput = true;
            waitUserTargetSelection = false;
        }
        else if (enemyUnitList.Contains(currentActiveUnit))
        {
            commandButtonPanel.SetActive(false);
        }
    }

    private void userSelectionLogic()
    {
        keyInputDebug();
        if (waitUserInput && !waitUserTargetSelection)
        {
            waitCommandSelectInput();
        }
        else if (waitUserInput && waitUserTargetSelection)
        {
            waitTargetSelectInput();
        }
    }

    private void keyInputDebug()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Debug.Log("up arrow");
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Debug.Log("down arrow");
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Debug.Log("right arrow");
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Debug.Log("left arrow");
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("return");
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("escapr");
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            Debug.Log("backspace");
        }
    }

    private void waitCommandSelectInput()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentSelectedButton == null)
            {
                currentSelectedButton = attackButton;
                selectButton();
            }
            else
            {
                unselectButton();
                if (currentSelectedButton == attackButton)
                {
                    currentSelectedButton = skillButton;
                }
                else if (currentSelectedButton == skillButton)
                {
                    currentSelectedButton = attackButton;
                }
                else if (currentSelectedButton == guardButton)
                {
                    currentSelectedButton = escapeButton;
                }
                else if (currentSelectedButton == escapeButton)
                {
                    currentSelectedButton = guardButton;
                }
                selectButton();
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentSelectedButton == null)
            {
                currentSelectedButton = attackButton;
                selectButton();
            }
            else
            {
                unselectButton();
                if (currentSelectedButton == attackButton)
                {
                    currentSelectedButton = guardButton;
                }
                else if (currentSelectedButton == skillButton)
                {
                    currentSelectedButton = escapeButton;
                }
                else if (currentSelectedButton == guardButton)
                {
                    currentSelectedButton = attackButton;
                }
                else if (currentSelectedButton == escapeButton)
                {
                    currentSelectedButton = skillButton;
                }
                selectButton();
            }
        }
        else if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
        {
            currentSelectedButton.button.onClick.Invoke();
        }
    }

    // TODO : 나중에는 Button에 class 스크립트 붙여서 퍼블릭 프로퍼티로 아웃라인 넣고 어쩌고 해서 관리할 것
    private void unselectButton()
    {
        var outline = currentSelectedButton.outLine;
        outline.effectDistance = new Vector2(1, 1);
    }

    private void selectButton()
    {
        var outline = currentSelectedButton.outLine;
        outline.effectDistance = new Vector2(10, 10);
    }

    private void initiateButtonOnClickEvent()
    {
        commandButtonPanel.SetActive(true);
        attackButton.button.onClick.AddListener(SelectAttack);
        skillButton.button.onClick.AddListener(SelectSkill);
        guardButton.button.onClick.AddListener(DoGuard);
        escapeButton.button.onClick.AddListener(TryEscape);
    }

    private void doMonsterLogic()
    {
        if (enemyUnitList.Contains(currentActiveUnit))
        {
            ((Enemy)currentActiveUnit).actionLogic.Invoke(BattleGlobal.userUnitList, enemyUnitList);
            changeCurrentActiveUnit();
        }
    }


    // TODO : 선택된 타겟의 머리 위에 뭐 indicator 같은거 보여줘야함.
    private void waitTargetSelectInput()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (isTargetSingle)
                SelectNextTarget(1);
            else
                SelectEveryTarget(selectFromType);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (isTargetSingle)
                SelectNextTarget(-1);
            else
                SelectEveryTarget(selectFromType);
        }
        else if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Escape))
        {
            UnselectTarget();
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            if (isTargetSingle)
                ActivateSkill(new List<Unit>() { currentSelectedTarget });
            else
                ActivateSkill(currentSelectedTargetList);
        }
    }

    private void SelectNextTarget(int offset)
    {
        if (isTargetSingle)
        {
            List<Unit> unitList = null;
            if (selectFromType == TargetSelectionType.ALLY)
            {
                unitList = BattleGlobal.userUnitList;
            }
            else if (selectFromType == TargetSelectionType.ENEMY)
            {
                unitList = enemyUnitList;
            }
            else if (selectFromType == TargetSelectionType.ALL)
            {
                unitList = totalUnitList;
            }

            if (unitList != null)
            {
                unsetIndicatorFromSelectedTarget(currentSelectedTarget);
                if (currentSelectedTarget == null)
                {
                    if (offset > 0)
                        currentSelectedTarget = unitList.First();
                    else
                        currentSelectedTarget = unitList.Last();
                }
                else
                {
                    var idx = unitList.IndexOf(currentSelectedTarget);
                    idx = GetNextIndex(idx, offset, unitList.Count());
                    currentSelectedTarget = unitList[idx];
                }
                setIndicatorToSelectedTarget(currentSelectedTarget);
            }
        }
    }

    private void SelectEveryTarget(TargetSelectionType type)
    {
        if (currentSelectedTargetList == null)
        {
            switch (type)
            {
                case TargetSelectionType.ENEMY:
                    currentSelectedTargetList = enemyUnitList;
                    break;
                case TargetSelectionType.ALLY:
                    currentSelectedTargetList = BattleGlobal.userUnitList;
                    break;
                case TargetSelectionType.ALL:
                    currentSelectedTargetList = totalUnitList;
                    break;
                default:
                    break;
            }
            setIndicatorToSelectedTarget(currentSelectedTargetList);
        }
    }

    private void UnselectTarget()
    {
        unsetIndicatorFromSelectedTarget(currentSelectedTarget);
        unsetIndicatorFromSelectedTarget(currentSelectedTargetList);
        currentSelectedTarget = null;
        currentSelectedTargetList.Clear();
        turnBackToCommandUI();
    }

    private void ActivateSkill(List<Unit> target)
    {
        if (target == null || target.Count <= 0)
            return;

        unsetIndicatorFromSelectedTarget(target);
        if (isUsingSkill)
        {
            UseSkill(currentActiveUnit, target);
        }
        else
        {
            DoAttack(currentActiveUnit, target[0]);
        }
    }

    private int GetNextIndex(int from, int offset, int size)
    {
        var idx = (from + offset) % size;
        while (idx < 0)
        {
            idx += size;
        }
        return idx;
    }

    private void setIndicatorToSelectedTarget(Unit target)
    {
        if (target != null)
            target.outline.effectDistance = new Vector2(15, 15);
    }
    private void setIndicatorToSelectedTarget(List<Unit> targetList)
    {
        if (targetList.Count != 0)
            targetList.ForEach(u => setIndicatorToSelectedTarget(u));
    }
    private void unsetIndicatorFromSelectedTarget(Unit target)
    {
        if (target != null)
            target.outline.effectDistance = new Vector2(1, 1);
    }
    private void unsetIndicatorFromSelectedTarget(List<Unit> targetList)
    {
        if (targetList.Count != 0)
            targetList.ForEach(u => unsetIndicatorFromSelectedTarget(u));
    }
}