import i18n from "i18next";
import { initReactI18next } from "react-i18next";

import English from "./en.json";
import Danish from "./da.json";

const resources = {
    da: {
        messages: Danish,
    },
    en: {
        messages: English,
    },
};

i18n.use(initReactI18next)
    .init({
        react: {
            useSuspense: true,
        },
        resources: resources,
        keySeparator: ".",
        ns: ["messages"],
        defaultNS: "messages",
        fallbackLng: "en",
        debug: true,
        // backend: {loadPath: '/public/locales/{{lng}}/{{ns}}.json'},

        interpolation: {
            escapeValue: false, // not needed for react as it escapes by default
        },
    })
    .catch((error) => {
        console.log(error);
    });

export default i18n;
