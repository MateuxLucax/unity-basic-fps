using Hero;
using UnityEngine;
using UnityEngine.AI;
using Weapons;

namespace Enemies.Boss
{
    public class Boss : MonoBehaviour, ITakeDamage
    {

        private NavMeshAgent _agent;
        private GameObject _player;
        private Inventory _inventory;
        private Animator _animator;
        public float attackDistance = 2.0f;
        private static readonly int Shot = Animator.StringToHash("shot");
        private static readonly int Attack = Animator.StringToHash("attack");
        private static readonly int CanWalk = Animator.StringToHash("canWalk");
        private static readonly int Capoeira = Animator.StringToHash("capoeira");
        private static readonly int StopAttack = Animator.StringToHash("stopAttack");
        public int healthPoints = 100;
        private const int Damage = 20;
        private Rigidbody _rigidbody;
        private RagDollEffect _ragDollEffect;
        public AudioClip deathSound;
        public AudioClip stepsSound;
        public AudioClip attackSound;
        public AudioClip damageSound;
        public AudioClip capoeiraSound;
        public AudioClip growlSound;
        private AudioSource _audioSource;
        private FieldOfView _fov;
        private RandomPatrol _randomPatrol;
        private bool _hasSeenPlayer;

        // Start is called before the first frame update
        void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _agent = GetComponent<NavMeshAgent>();
            _player = GameObject.FindWithTag("Player");
            _inventory = _player.GetComponent<Inventory>();
            _animator = GetComponent<Animator>();
            _ragDollEffect = GetComponent<RagDollEffect>();
            _audioSource = GetComponent<AudioSource>();
            _ragDollEffect.Init();
            _fov = GetComponent<FieldOfView>();
            _randomPatrol = GetComponent<RandomPatrol>();
        }

        // Update is called once per frame
        void Update()
        {
            if (healthPoints <= 0)
            {
                Die();
                return;
            }

            if (_fov.canSeePlayer || _animator.GetBool(StopAttack) == false)
            {
                if (!_hasSeenPlayer)
                {
                    PlayGrowl();
                    _hasSeenPlayer = true;
                }
                RunTowardsPlayer();
            } else
            {
                _hasSeenPlayer = false;
                _animator.SetBool(StopAttack, true);
                CorrectRigidbodyExit();
                _agent.isStopped = false;
                _randomPatrol.Walk();
            }
        }

        private void PlayGrowl()
        {
            _audioSource.PlayOneShot(growlSound);
        }

        private void RunTowardsPlayer()
        {
            var distanceFromPlayer = Vector3.Distance(transform.position, _player.transform.position);
            if (distanceFromPlayer < attackDistance)
            {
                _agent.isStopped = true;
                _animator.SetTrigger(Random.value > 0.5f ? Capoeira : Attack);
                _animator.SetBool(CanWalk, false);
                _animator.SetBool(StopAttack, false);
                CorrectRigidbodyEnter();
            }
            if (distanceFromPlayer >= attackDistance + 1)
            {
                _animator.SetBool(StopAttack, true);
                CorrectRigidbodyExit();
            }
            if (_animator.GetBool(CanWalk))
            {
                _agent.isStopped = false;
                _animator.ResetTrigger(Attack);
                _animator.ResetTrigger(Capoeira);
                _agent.SetDestination(_player.transform.position);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                CorrectRigidbodyEnter();
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                CorrectRigidbodyExit();
            }
        }

        private void CorrectRigidbodyEnter()
        {
            _rigidbody.isKinematic = true;
            _rigidbody.velocity = Vector3.zero;
        }

        private void CorrectRigidbodyExit()
        {
            _rigidbody.isKinematic = false;
        }

        public void TakeDamage(int damage)
        {
            _audioSource.PlayOneShot(damageSound);
            healthPoints -= damage;
            _agent.isStopped = true;
            _animator.SetTrigger(Shot);
            _animator.SetBool(CanWalk, false);
            if (healthPoints <= 0)
            {
                Die();
            }
        }

        public void Step()
        {
            _audioSource.PlayOneShot(stepsSound);
        }

        public void DoDamage()
        {
            _audioSource.PlayOneShot(Random.value > 0.5 ? capoeiraSound : attackSound);
            _player.GetComponent<PlayerMovementController>().UpdateHealth(-Damage);
        }

        private void Die()
        {
            _inventory.KilledBoss();
            _audioSource.clip = deathSound;
            _audioSource.Play();
            _agent.isStopped = true;
            _animator.SetBool(CanWalk, false);
            _ragDollEffect.Activate();
            enabled = false;
            _fov.enabled = false;
        }
    }
}
