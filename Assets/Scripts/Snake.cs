/**
    MIT License
    Copyright (c) 2021 Kale Ko
    See https://kaleko.ga/license.txt
*/

using UnityEngine;
using System.Collections.Generic;

public class Snake : MonoBehaviour
{
    public GameObject bodyPrefab;

    private Game game;
    public bool started = false;

    private Vector3 moveddirrection = new Vector3(0.5f, 0, 0);
    private Vector3 dirrection = new Vector3(0.5f, 0, 0);
    private float speed = 1;
    private bool grow = false;

    public int length = 1;
    private List<GameObject> body = new List<GameObject>();

    private Touch[] previousTouches;

    void Start()
    {
        this.game = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>();
    }

    void Update()
    {
        if (!this.started && (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.touchCount > 0))
        {
            this.started = true;

            Invoke("Move", 1 / this.speed);
        }

        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && this.moveddirrection != new Vector3(0, -0.5f, 0)) this.dirrection = new Vector3(0, 0.5f, 0);
        if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && this.moveddirrection != new Vector3(0, 0.5f, 0)) this.dirrection = new Vector3(0, -0.5f, 0);
        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && this.moveddirrection != new Vector3(0.5f, 0, 0)) this.dirrection = new Vector3(-0.5f, 0, 0);
        if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && this.moveddirrection != new Vector3(-0.5f, 0, 0)) this.dirrection = new Vector3(0.5f, 0, 0);

        if ((Input.mousePresent && Input.simulateMouseWithTouches) || Input.touchSupported || Input.stylusTouchSupported)
        {
            foreach (Touch touch in Input.touches)
            {
                foreach (Touch previoustouch in this.previousTouches)
                {
                    if (touch.fingerId == previoustouch.fingerId)
                    {
                        if (touch.position.y < previoustouch.position.y && (touch.deltaPosition.y < 0 ? -touch.deltaPosition.y : touch.deltaPosition.y) > (touch.deltaPosition.x < 0 ? -touch.deltaPosition.x : touch.deltaPosition.x)) this.dirrection = new Vector3(0, -0.5f, 0);
                        if (touch.position.y > previoustouch.position.y && (touch.deltaPosition.y < 0 ? -touch.deltaPosition.y : touch.deltaPosition.y) > (touch.deltaPosition.x < 0 ? -touch.deltaPosition.x : touch.deltaPosition.x)) this.dirrection = new Vector3(0, 0.5f, 0);
                        if (touch.position.x < previoustouch.position.x && (touch.deltaPosition.x < 0 ? -touch.deltaPosition.x : touch.deltaPosition.x) > (touch.deltaPosition.y < 0 ? -touch.deltaPosition.y : touch.deltaPosition.y)) this.dirrection = new Vector3(-0.5f, 0, 0);
                        if (touch.position.x > previoustouch.position.x && (touch.deltaPosition.x < 0 ? -touch.deltaPosition.x : touch.deltaPosition.x) > (touch.deltaPosition.y < 0 ? -touch.deltaPosition.y : touch.deltaPosition.y)) this.dirrection = new Vector3(0.5f, 0, 0);
                    }
                }
            }

            this.previousTouches = Input.touches;
        }
    }

    void Move()
    {
        if (!this.started) return;

        this.moveddirrection = this.dirrection;
        Vector3 prevpos = transform.position;

        transform.position += dirrection;

        if (grow)
        {
            GameObject newbody = Instantiate(bodyPrefab);
            newbody.name = "Body";
            newbody.transform.position = prevpos;

            this.body.Insert(0, newbody);

            grow = false;
        }
        else if (this.length > 1)
        {
            this.body.ToArray()[this.body.Count - 1].transform.position = prevpos;

            this.body.Insert(0, this.body[this.body.Count - 1]);
            this.body.RemoveAt(this.body.Count - 1);
        }

        if (this.dirrection == new Vector3(0.5f, 0, 0)) transform.rotation = Quaternion.Euler(0, 0, -90);
        if (this.dirrection == new Vector3(-0.5f, 0, 0)) transform.rotation = Quaternion.Euler(0, 0, 90);
        if (this.dirrection == new Vector3(0, 0.5f, 0)) transform.rotation = Quaternion.Euler(0, 0, 0);
        if (this.dirrection == new Vector3(0, -0.5f, 0)) transform.rotation = Quaternion.Euler(0, 0, 180);

        this.speed += 0.0025f;

        Invoke("Move", 1 / this.speed);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.name.Contains("Wall") || collider.gameObject.name == "Body")
        {
            this.started = false;

            game.GameOver();
        }
        else if (collider.gameObject.name == "Food")
        {
            this.length++;

            this.speed += 0.25f;

            Destroy(collider.gameObject);

            this.grow = true;

            GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>().SpawnFood();
        }
    }
}