# ReinforcementLab
A reinforcement learning testground made for unity

**Universidade de Brasília**

**Departamento de Ciência da Computação**

**CIC0193 - Fundamentos de Sistemas Inteligentes - 2021/1**

**Alunos:**

-   Artur H. C. Pereira
-   Felipe O. M. Neves

**Professor:**  Vinícius R. P. Borges

Esse projeto foi desenvolvido para a disciplina de Fundamento de Sistemas inteligentes. A proposta do projeto é desenvolver um ambiente interativo para avaliação de algoritmos de aprendizado por reforço em um problema de solução de labirinto.

O sistema desenvolvido está disponível no endereço https://arturhugo.github.io/QMazing-WebGL/

## Estrutura do código

O código está estruturado na seguinte estrutura de pastas:

- /Game: scripts de funcionamento do simulador (geração do labirinto, movimentação do agente)
- /HUD: scripts para menu para usuário ajustar e testar parâmetros da simulação
- /Learning: scripts dos agentes de aprendizado por reforço

## Manual

### Menu de agente
No canto esquerdo superior, encontra-se o menu de seleção do agente de aprendizado. Nesse menu é possível ver qual agente está sendo usado e trocar para o próximo agente. Atualmente estão implementados os agentes [Q-Learning](https://en.wikipedia.org/wiki/Q-learning) e [SARSA](https://en.wikipedia.org/wiki/State%E2%80%93action%E2%80%93reward%E2%80%93state%E2%80%93action).

### Menu de hiperparâmetros
No canto esquerdo inferior, encontra-se o menu de hiperparâmetros do agente. Nesse menu é possível selecionar os valores para cada hiperparâmetro do agente atualmente utilizado.

### Menu de labirinto
No canto direito superior, encontra-se o menu do labirinto, onde é possível ver o tamanho do lado do labirinto em células e gerar um novo labirinto com diferentes tamanhos.

### Menu de simulação e métricas
Por fim, no canto direito inferior está o menu da simulação, onde pode-se selecionar o número de episódios e número de iterações por episódio que serão executados. Além disso, nesse menu é possível visualizar a janela de métricas, onde são exibidos os gráficos de número de passos até o objetivo, recompensa acumulada e recompensa média, todos relativos ao número do episódio.

Na janela de métricas é possível criar uma nova página para exibir um novo conjunto de gráficos. Dessa forma, pode-se realizar a comparação entre duas execuções diferentes. Assim é possível analisar o impacto que as mudanças feitas em cada hiperparâmetro causam nas métricas exibidas ou como dois agentes diferentes se comportam para um mesmo conjunto de hiperparâmetros.

### Treinamento e execução passo a passo

Para realizar um processo de treinamento efetivo para o agente, é ideal colocar um número maior de episódios e iterações. Porém, a animação do agente resolvendo o labirinto não será exibida nesse caso.

Para visualizar o agente resolvendo o labirinto, deve-se colocar apenas um episódio no menu de simulação e um número um pouco mais reduzido de iterações.

É possível, também, executar apenas um episódio com uma iteração por vez para ver o processo passo a passo. Nesse modo de execução, é habilitada uma interface de setas que mostram as recompensas que o agente enxerga para casa ação de cada estado em que ele se encontra.

Também é possível usar as teclas WASD para explorar o labirinto diretamente.

Antes de utilizar outro modo de execução, é importante reiniciar a simulação para o estado inicial.
