[![CircleCI](https://circleci.com/gh/axle-h/Axh.Fit.Endomondo/tree/master.svg?&style=shield)](https://circleci.com/gh/axle-h/Axh.Fit.Endomondo/tree/master)

> Endomondo [is dead](https://support.endomondo.com/hc/en-us/articles/360016251359-Endomondo-Is-Retired) 😭
> I can't even login anymore so this tool is useless!

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