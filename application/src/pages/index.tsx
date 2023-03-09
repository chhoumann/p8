import { type NextPage } from "next";
import Head from "next/head";
import { useTranslation } from "react-i18next";
import "../i18n";

const Home: NextPage = () => {
    const { t } = useTranslation();

    return (
        <>
            <Head>
                <title>P8 Application</title>
                <meta name="description" content="" />
                {/* <link rel="icon" href="/favicon.ico" /> */}
            </Head>
            <main className="">{t("example.hello")}</main>
        </>
    );
};

export default Home;
