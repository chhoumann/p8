import React from "react";
import { useTranslation } from "react-i18next";
import "../i18n";

function LocalizationExample(): JSX.Element {
    const { t } = useTranslation();

    return <div>{t("example.hello")}</div>;
}

export default LocalizationExample;
