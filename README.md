# Axh.Fit.Endomondo

Endomondo workout scraper.

## Usage

Go to Endomondo website, login and save the value of your USER_TOKEN cookie. This does expire so if you start seeing 401 exceptions, get a new one.
To scrape all workouts as GPX files:

```
Axh.Fit.Endomondo -gu endomondo_user_id
```

Or as TCX:

```
Axh.Fit.Endomondo -tu endomondo_user_id
```