# Congestion Tax Services

- [Congestion Tax Api](congestion-tax-api)
 - [Calculate Congestion Tax](#calculate-congestion-tax)
    - [Calculate Congestion Tax Request](#calculate-congestion-tax-request)
    - [Calculate Congestion Response](#calculate-congestion-tax-response)


## Calculate Congestion Tax

### Calculate Congestion Tax Request

```js
POST /congestion-tax
```

```json
{
    "vehicle": {

    },

    "dates": [
            "2013-01-14 21:00:00"
            "2013-01-15 21:00:00"
            "2013-02-07 06:23:27"
            "2013-02-07 15:27:00"
            "2013-02-08 06:27:00"
            "2013-02-08 06:20:27"
            "2013-02-08 14:35:00"
            "2013-02-08 15:29:00"
            "2013-02-08 15:47:00"
            "2013-02-08 16:01:00"
            ]
}
```

### Calculate Congestion Tax Response

```js
200 OK
```

```json
{
    "tax": " "
}
```
