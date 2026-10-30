"""Telegram-бот: следит за HTX P2P и присылает самые дешёвые объявления."""
import os
import time
import requests

BOT_TOKEN = os.environ["BOT_TOKEN"]
CHAT_ID = os.environ.get("CHAT_ID")  # куда слать автоуведомления (можно узнать через /start)
COIN_ID = os.environ.get("COIN_ID", "2")        # 2 = USDT
CURRENCY = os.environ.get("CURRENCY", "11")     # 11 = RUB
TRADE_TYPE = os.environ.get("TRADE_TYPE", "sell")  # sell = вкладка «Покупка» (продавцы продают вам)
PAY_METHOD = os.environ.get("PAY_METHOD", "0")  # 0 = любой
TOP_N = int(os.environ.get("TOP_N", "5"))
CHECK_INTERVAL = int(os.environ.get("CHECK_INTERVAL", "60"))
MAX_PRICE = float(os.environ.get("MAX_PRICE", "0"))  # 0 = уведомлять при любом изменении лучшей цены
MIN_LIMIT = float(os.environ.get("MIN_LIMIT", "0"))  # пропускать объявления с минимальным лимитом выше этого (0 = выкл)
# Фильтры из приложения: «Доступные» включено по умолчанию, остальные выкл.
ONLY_TRADABLE = os.environ.get("ONLY_TRADABLE", "true")  # «Доступные»
THUMBS_UP = os.environ.get("THUMBS_UP", "false")         # «Thumbs Up»
VERIFIED = os.environ.get("VERIFIED", "false")           # «Проверенные»
TRADED = os.environ.get("TRADED", "false")               # «С кем недавно торговал»
FOLLOWED = os.environ.get("FOLLOWED", "false")           # «Отслеживаемые»
API_URL = os.environ.get("HTX_API_URL", "https://www.htx.com/-/x/otc/v1/data/trade-market")

TG = f"https://api.telegram.org/bot{BOT_TOKEN}"
HEADERS = {"User-Agent": "Mozilla/5.0"}


def fetch_ads():
    params = {
        "coinId": COIN_ID, "currency": CURRENCY, "tradeType": TRADE_TYPE,
        "currPage": 1, "payMethod": PAY_METHOD, "acceptOrder": 0,
        "blockType": "general", "online": 1, "range": 0, "amount": "",
        "onlyTradable": ONLY_TRADABLE, "isThumbsUp": THUMBS_UP, "isMerchant": VERIFIED,
        "isTraded": TRADED, "isFollowed": FOLLOWED,
    }
    r = requests.get(API_URL, params=params, headers=HEADERS, timeout=15)
    r.raise_for_status()
    ads = r.json().get("data") or []
    out = []
    for a in ads:
        try:
            price = float(a["price"])
            lo, hi = float(a["minTradeLimit"]), float(a["maxTradeLimit"])
        except (KeyError, ValueError, TypeError):
            continue
        if MIN_LIMIT and lo > MIN_LIMIT:
            continue
        out.append({
            "name": a.get("userName", "?"), "price": price, "lo": lo, "hi": hi,
            "qty": a.get("tradeCount", "?"),
            "orders": a.get("tradeMonthTimes", "?"),
            "pay": ", ".join(p.get("name", "") for p in a.get("payMethods", [])),
        })
    out.sort(key=lambda x: x["price"])
    return out[:TOP_N]


def fmt(ads):
    if not ads:
        return "Объявлений не найдено."
    lines = ["Самые дешёвые P2P HTX:"]
    for i, a in enumerate(ads, 1):
        lines.append(
            f"{i}. ₽{a['price']:.2f} — {a['name']}\n"
            f"   лимиты ₽{a['lo']:.0f}–₽{a['hi']:.0f}, объём {a['qty']}, сделок {a['orders']}\n"
            f"   {a['pay']}"
        )
    return "\n".join(lines)


def send(chat_id, text):
    requests.post(f"{TG}/sendMessage", json={"chat_id": chat_id, "text": text}, timeout=15)


def handle_updates(offset):
    r = requests.get(f"{TG}/getUpdates", params={"offset": offset, "timeout": 5}, timeout=20).json()
    for u in r.get("result", []):
        offset = u["update_id"] + 1
        msg = u.get("message") or {}
        text, chat = msg.get("text", ""), msg.get("chat", {}).get("id")
        if not chat:
            continue
        if text.startswith("/start"):
            send(chat, f"Привет! Ваш chat_id: {chat}\nЗадайте его в CHAT_ID для автоуведомлений.\n/top — показать топ сейчас.")
        elif text.startswith("/top"):
            try:
                send(chat, fmt(fetch_ads()))
            except Exception as e:
                send(chat, f"Ошибка запроса к HTX: {e}")
    return offset


def main():
    offset, last_best, last_check = None, None, 0
    while True:
        offset = handle_updates(offset)
        if CHAT_ID and time.time() - last_check >= CHECK_INTERVAL:
            last_check = time.time()
            try:
                ads = fetch_ads()
                best = ads[0]["price"] if ads else None
                if best is not None and best != last_best and (not MAX_PRICE or best <= MAX_PRICE):
                    send(CHAT_ID, fmt(ads))
                last_best = best
            except Exception as e:
                print("check error:", e)


if __name__ == "__main__":
    main()
