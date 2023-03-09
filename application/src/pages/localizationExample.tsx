import React from "react";
import { useTranslation } from "react-i18next";
import i18n from "../i18n";

function LocalizationExample(): JSX.Element {
    const { t } = useTranslation();

    return (
        <div>
            <div>{t("example.hello")}</div>
            <button
                onClick={() =>
                    i18n.changeLanguage(i18n.language === "da" ? "en" : "da")
                }
            >
                {t("example.changeLanguage")}
            </button>
        </div>
    );
}

export default LocalizationExample;
