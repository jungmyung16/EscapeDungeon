using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Type { Melee }; // 占쏙옙占쏙옙 타占쏙옙: 占쏙옙占쏙옙占쏙옙占쏙옙
    public Type type;

    public int damage;                  // 占쏙옙占쏙옙占쏙옙
    public float rate;                  // 占쏙옙占쏙옙 占쌈듸옙
    public BoxCollider meleeArea;       // 占쏙옙占쏙옙 占쏙옙占쏙옙
    public TrailRenderer trialEffect; // 효占쏙옙 占쏙옙占쏙옙

    public void Use()
    {
        if(type == Type.Melee)
        {
            StopCoroutine("Swing");
            StartCoroutine("Swing");
        }
    }
    // 占쌘뤄옙틴占쏙옙占쏙옙 占쏙옙占쏙옙 占쏙옙占쏙옙 占쏙옙占쏙옙
    // yield 키占쏙옙占쏙옙占? 占쏙옙占쏙옙 占쌜쇽옙 占쏙옙占쏙옙
    IEnumerator Swing()
    {
        yield return new WaitForSeconds(0.1f);  // 0.1占쏙옙 占쏙옙占?
        meleeArea.enabled = true;
        trialEffect.enabled = true;

        yield return new WaitForSeconds(0.3f);  // 0.3占쏙옙 占쏙옙占?
        meleeArea.enabled = false;

        yield return new WaitForSeconds(0.3f);  // 0.3占쏙옙 占쏙옙占?
        trialEffect.enabled = false;
    }
}
