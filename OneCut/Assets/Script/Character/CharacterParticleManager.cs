using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eEffect
{
	eNull = -1,
	eLevelUp = 0,	// 레벨업 이팩트.
}

public class CharacterParticleManager : MonoSingleton<CharacterParticleManager> 
{
	public Transform m_tfCharacter;
	public List<ParticleSystem> m_listEffects;

	public ParticleSystem GetEffect(eEffect type)
	{
		int index = (int)type;
		return m_listEffects [index];
	}

	public void PlayEffect(eEffect type)
	{
		this.transform.position = m_tfCharacter.position;

		switch (type) {
		case eEffect.eLevelUp:
			this.transform.position += new Vector3 (0f, -0.4f, 0f);
			break;
		}

		ParticleSystem pts = GetEffect (type);
		if (pts.isPlaying == true) {
			pts.Stop ();
		}
		pts.Play ();
	}
}
