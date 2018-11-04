using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveablePlatform : MonoBehaviour {

    //Public Variables
    public Vector2 direction_in;
    public float acceleration_in;
    public float max_speed;
    public float distance;

    //Private Variables
    float acceleration;
    Vector2 accel_direction;
    Vector2 velocity;
    Vector3 pos;
    Vector3 original_pos;
    Vector2 dead_zone_begin;
    Vector2 dead_zone_end;
    Vector2 half_way;

    void Start()
    {
        dead_zone_begin = Vector2.zero;
        accel_direction = direction_in;
        original_pos = transform.position;
        half_way = new Vector2(original_pos.x + (direction_in.x * (distance/2)), original_pos.y + (direction_in.y * (distance / 2)));
    }

    void FixedUpdate()
    {
        pos = transform.position;
        if (acceleration_in == 0)
        {
            if ((Mathf.Abs(pos.x - original_pos.x) >= distance) //At extreme pos x
               | (Mathf.Abs(pos.y - original_pos.y) >= distance)) //At extreme pos y 
            {
                velocity = -direction_in * max_speed;
            }
            if ((pos - original_pos) == Vector3.zero)
            {
                velocity = direction_in * max_speed;
            }
        }

        else
        {
            if ((Mathf.Abs(pos.x - original_pos.x) >= distance / 2) // If past halfway point
            | (Mathf.Abs(pos.y - original_pos.y) >= distance / 2))
            {
                accel_direction = -direction_in;
            }
            else
            {
                accel_direction = direction_in;
            }
            if ((Mathf.Abs(velocity.x) >= max_speed | Mathf.Abs(velocity.y) >= max_speed) && dead_zone_begin == Vector2.zero)
            {
                dead_zone_begin = pos;
                dead_zone_end = half_way + (half_way - dead_zone_begin);
            }
            if (pos.x < dead_zone_begin.x | pos.x > dead_zone_end.x
                | pos.y < dead_zone_begin.y | pos.y > dead_zone_end.y)
            {
                velocity += (acceleration_in * Time.deltaTime * accel_direction);
            }
        }
        transform.Translate(Time.deltaTime * new Vector3(velocity.x, velocity.y, 0));
    }
}
