using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { START, PLAYERTURN, ENEMYSELECT, WAIT, ENEMYTURN, WON, LOST}

public class BattleSystem : MonoBehaviour
{
    public GameObject beanPrefab;
    public GameObject enemyPrefab;

    public Transform beanSpot;
    public Transform enemySpot1;

    public Transform attackNode1;
    public Transform enemyAttackNode;

    Unit beanUnit;
    Unit enemyUnit;

    public BeanBattleHUD beanHUD;
    public BattleHUD enemyHUD;

    public BattleState state;

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        // Set the scene and the UI up for the battle
        GameObject beanGO = Instantiate(beanPrefab, beanSpot);
        beanUnit = beanGO.GetComponent<Unit>();

        GameObject enemyGO = Instantiate(enemyPrefab, enemySpot1);
        enemyUnit = enemyGO.GetComponent<Unit>();

        beanHUD.SetHUD(beanUnit);
        enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(2f);

        // Initiate the player's turn
        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    void PlayerTurn()
    {

    }

    IEnumerator PlayerAttack()
    {
        // Perform an attack
        bool isDead = enemyUnit.TakeDamage(beanUnit.damage);
        enemyHUD.SetHP(enemyUnit.currentHP);
        state = BattleState.WAIT;

        yield return new WaitForSeconds(2f);
        // Check if enemy is defeated and change BattleState based on result
        if(isDead)
        {
            // End the battle, with the player as the victor
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            // Initiate the enemy's turn
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator PlayerHeal()
    {
        // Restore HP, checking if maximum will be reached. If so, only restore enough to reach the maximum
        beanUnit.Heal(5);
        beanHUD.SetHP(beanUnit.currentHP);
        state = BattleState.WAIT;

        yield return new WaitForSeconds(2f);
        // Change BattleState
        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(1f);

        bool isDead = beanUnit.TakeDamage(enemyUnit.damage);
        beanHUD.SetHP(beanUnit.currentHP);

        yield return new WaitForSeconds(1f);

        // Check if Bean is defeated and change the BattleState based on result
        if(isDead)
        {
            // End the battle, with the player as the loser
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            // Initiate the player's turn
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    void EndBattle()
    {
        if (state == BattleState.WON)
        {

        } else if (state == BattleState.LOST)
        {

        }
    }

    public void OnAttackButton()
    {
        // If it's not the player's turn, don't let them attack
        if (state != BattleState.PLAYERTURN)
            return;
        // If it is the player's turn, proceed
        StartCoroutine(PlayerAttack());
    }

    public void OnHealButton()
    {
        // If it's not the player's turn, don't let them attack
        if (state != BattleState.PLAYERTURN)
            return;
        StartCoroutine(PlayerHeal());
    }
}
