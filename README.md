# Axh.Fit.Endomondo

Endomondo workout scraper.

## Usage

Go to Endomondo website, login and save the value of your USER_TOKEN cookie.
To scrape all workouts as GPX files:

```
Axh.Fit.Endomondo -gu endomondo_user_token
```

Or as TCX:

```
Axh.Fit.Endomondo -tu endomondo_user_token
```

Or both:

```
Axh.Fit.Endomondo -gtu endomondo_user_token
```

## Errors

* **401** Your cookie has expired, login to Endomondo again and take a fresh copy of your USER_TOKEN.
* **429** You've made too many requests. It looks like Endomondo has a request limit. Just wait a minute or two and try again.