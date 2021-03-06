# multiAgents.py
# --------------
# Licensing Information:  You are free to use or extend these projects for
# educational purposes provided that (1) you do not distribute or publish
# solutions, (2) you retain this notice, and (3) you provide clear
# attribution to UC Berkeley, including a link to http://ai.berkeley.edu.
# 
# Attribution Information: The Pacman AI projects were developed at UC Berkeley.
# The core projects and autograders were primarily created by John DeNero
# (denero@cs.berkeley.edu) and Dan Klein (klein@cs.berkeley.edu).
# Student side autograding was added by Brad Miller, Nick Hay, and
# Pieter Abbeel (pabbeel@cs.berkeley.edu).


from util import manhattanDistance
from game import Directions
import random, util

from game import Agent

class ReflexAgent(Agent):
    """
    A reflex agent chooses an action at each choice point by examining
    its alternatives via a state evaluation function.

    The code below is provided as a guide.  You are welcome to change
    it in any way you see fit, so long as you don't touch our method
    headers.
    """


    def getAction(self, gameState):
        """
        You do not need to change this method, but you're welcome to.

        getAction chooses among the best options according to the evaluation function.

        Just like in the previous project, getAction takes a GameState and returns
        some Directions.X for some X in the set {NORTH, SOUTH, WEST, EAST, STOP}
        """
        # Collect legal moves and successor states
        legalMoves = gameState.getLegalActions()

        # Choose one of the best actions
        scores = [self.evaluationFunction(gameState, action) for action in legalMoves]
        bestScore = max(scores)
        bestIndices = [index for index in range(len(scores)) if scores[index] == bestScore]
        chosenIndex = random.choice(bestIndices) # Pick randomly among the best

        "Add more of your code here if you want to"

        return legalMoves[chosenIndex]

    def evaluationFunction(self, currentGameState, action):
        """
        Design a better evaluation function here.

        The evaluation function takes in the current and proposed successor
        GameStates (pacman.py) and returns a number, where higher numbers are better.

        The code below extracts some useful information from the state, like the
        remaining food (newFood) and Pacman position after moving (newPos).
        newScaredTimes holds the number of moves that each ghost will remain
        scared because of Pacman having eaten a power pellet.

        Print out these variables to see what you're getting, then combine them
        to create a masterful evaluation function.
        """
        # Useful information you can extract from a GameState (pacman.py)
        successorGameState = currentGameState.generatePacmanSuccessor(action)
        newPos = successorGameState.getPacmanPosition()
        newFood = successorGameState.getFood()
        newGhostStates = successorGameState.getGhostStates()
        newScaredTimes = [ghostState.scaredTimer for ghostState in newGhostStates]

        "*** YOUR CODE HERE ***"
        #Return a high value on a win
        if successorGameState.isWin():
            return 1000000

        #Return a low value on a loss
        if successorGameState.isLose():
            return -1000000

        value = 0

        #Find the closest food and closest ghost
        closestFood = min([manhattanDistance(newPos, food) for food in newFood.asList()])
        closestGhost = min([manhattanDistance(newPos, ghost.getPosition())
                               for ghost in newGhostStates])

        #Take reciprocal of closest food (closer foods better), and negatively weight states with
        #more remaining food
        value += float(1/closestFood)
        value -= successorGameState.getNumFood()
        #print(str(successorGamesState.getNumFood()))

        #Negatively weight states that become closer to ghosts by taking the negative reciprocal
        value -= 1/closestGhost

        #Penalize stopping
        if action == "Stop":
            value -= 100

        return successorGameState.getScore() + value


def scoreEvaluationFunction(currentGameState):
    """
    This default evaluation function just returns the score of the state.
    The score is the same one displayed in the Pacman GUI.

    This evaluation function is meant for use with adversarial search agents
    (not reflex agents).
    """
    return currentGameState.getScore()

class MultiAgentSearchAgent(Agent):
    """
    This class provides some common elements to all of your
    multi-agent searchers.  Any methods defined here will be available
    to the MinimaxPacmanAgent, AlphaBetaPacmanAgent & ExpectimaxPacmanAgent.

    You *do not* need to make any changes here, but you can if you want to
    add functionality to all your adversarial search agents.  Please do not
    remove anything, however.

    Note: this is an abstract class: one that should not be instantiated.  It's
    only partially specified, and designed to be extended.  Agent (game.py)
    is another abstract class.
    """

    def __init__(self, evalFn = 'scoreEvaluationFunction', depth = '2'):
        self.index = 0 # Pacman is always agent index 0
        self.evaluationFunction = util.lookup(evalFn, globals())
        self.depth = int(depth)

class MinimaxAgent(MultiAgentSearchAgent):
    """
    Your minimax agent (question 2)
    """

    def getAction(self, gameState):
        """
        Returns the minimax action from the current gameState using self.depth
        and self.evaluationFunction.

        Here are some method calls that might be useful when implementing minimax.

        gameState.getLegalActions(agentIndex):
        Returns a list of legal actions for an agent
        agentIndex=0 means Pacman, ghosts are >= 1

        gameState.generateSuccessor(agentIndex, action):
        Returns the successor game state after an agent takes an action

        gameState.getNumAgents():
        Returns the total number of agents in the game

        gameState.isWin():
        Returns whether or not the game state is a winning state

        gameState.isLose():
        Returns whether or not the game state is a losing state
        """
        "*** YOUR CODE HERE ***"
        def maxValue(state, agentIndex, depth):
            #If terminal-test then return utility
            if state.isWin() or state.isLose() or depth == self.depth:
                return self.evaluationFunction(state)

            #initialize v
            v = float('-inf')

            #for each successor of state, v=max(v,minValue(successor))
            agentIndex = 0
            actions = state.getLegalActions(agentIndex)
            for action in actions:
                v = max(v, minValue(state.generateSuccessor(agentIndex, action), agentIndex + 1, depth + 1))
            
            return v

        def minValue(state, agentIndex, depth):
            #If terminal-test then return utility
            if state.isWin() or state.isLose():
                return self.evaluationFunction(state)

            #initialize v
            v = float('inf')

            #for each successor of state v= min(v, maxValue(successor)).
            actions = state.getLegalActions(agentIndex)
            if agentIndex == (state.getNumAgents() - 1):
                for action in actions:
                    v = min(v, maxValue(state.generateSuccessor(agentIndex, action), agentIndex, depth))
            else:
                for action in actions:
                    v = min(v, minValue(state.generateSuccessor(agentIndex, action), agentIndex + 1, depth))

            return v

        actions = gameState.getLegalActions(0)
        #The base case for the recursion, start at the top of the tree, letting the mins act first.
        #Make a dictionary to keep track of each action and its value, return the action that has the max
        #value of the successor minValue values
        actionSet = {}
        for action in actions:
            actionSet[action] = minValue(gameState.generateSuccessor(0, action), 1, 1)
        return max(actionSet, key=actionSet.get)

class AlphaBetaAgent(MultiAgentSearchAgent):
    """
    Your minimax agent with alpha-beta pruning (question 3)
    """

    def getAction(self, gameState):
        """
        Returns the minimax action using self.depth and self.evaluationFunction
        """
        "*** YOUR CODE HERE ***"
        def maxValue(state, agentIndex, depth, alpha, beta):
            #If terminal-test then return utility
            if state.isWin() or state.isLose() or depth == self.depth:
                return self.evaluationFunction(state)

            #initialize v and curAlpha
            v = float('-inf')
            curAlpha = alpha

            #for each successor of state, v=max(v,minValue(successor))
            agentIndex = 0
            actions = state.getLegalActions(agentIndex)
            for action in actions:
                v = max(v, minValue(state.generateSuccessor(agentIndex, action),
                                    agentIndex + 1, depth + 1, curAlpha, beta))
                if v > beta:
                    return v
                curAlpha = max(curAlpha, v)
            
            return v

        def minValue(state, agentIndex, depth, alpha, beta):
            #If terminal-test then return utility
            if state.isWin() or state.isLose():
                return self.evaluationFunction(state)

            #initialize v and beta
            v = float('inf')
            curBeta = beta

            #for each successor of state v= min(v, maxValue(successor)).
            actions = state.getLegalActions(agentIndex)
            if agentIndex == (state.getNumAgents() - 1):
                for action in actions:
                    v = min(v, maxValue(state.generateSuccessor(agentIndex, action),
                                        agentIndex, depth, alpha, curBeta))
                    if v < alpha:
                        return v
                    curBeta = min(curBeta, v)
            else:
                for action in actions:
                    v = min(v, minValue(state.generateSuccessor(agentIndex, action),
                                        agentIndex + 1, depth, alpha, curBeta))
                    if v < alpha:
                        return v
                    curBeta = min(curBeta, v)

            return v

        #The base case for the recursion, start at the top of the tree, letting the mins act first.
        #Make a dictionary to keep track of each action and its value, return the action that has the max
        #value of the successor minValue values
        actions = gameState.getLegalActions(0)
        actionSet = {}
        alpha = float('-inf')
        beta = float('inf')
        for action in actions:
            v = minValue(gameState.generateSuccessor(0, action), 1, 1, alpha, beta)
            actionSet[action] = v
            if v > beta:
                return v;
            alpha = max(v, alpha)
        return max(actionSet, key=actionSet.get)

class ExpectimaxAgent(MultiAgentSearchAgent):
    """
      Your expectimax agent (question 4)
    """

    def getAction(self, gameState):
        """
        Returns the expectimax action using self.depth and self.evaluationFunction

        All ghosts should be modeled as choosing uniformly at random from their
        legal moves.
        """
        "*** YOUR CODE HERE ***"
        def maxValue(state, agentIndex, depth):
            #If terminal-test then return utility
            if state.isWin() or state.isLose() or depth == self.depth:
                return self.evaluationFunction(state)

            #initialize v
            v = float('-inf')

            #for each successor of state, v=max(v,expValue(successor))
            agentIndex = 0
            actions = state.getLegalActions(agentIndex)
            for action in actions:
                v = max(v, expValue(state.generateSuccessor(agentIndex, action), agentIndex + 1, depth + 1))
            
            return v

        def expValue(state, agentIndex, depth):
            #If terminal-test then return utility
            if state.isWin() or state.isLose():
                return self.evaluationFunction(state)

            #initialize v
            v = 0

            #for each successor of state v= min(v, maxValue(successor)).
            actions = state.getLegalActions(agentIndex)
            for action in actions:
                if agentIndex == (state.getNumAgents() - 1):
                    vSuccessor = maxValue(state.generateSuccessor(agentIndex, action),
                                                 agentIndex, depth)
                else:
                    vSuccessor = expValue(state.generateSuccessor(agentIndex, action),
                                                 agentIndex + 1, depth)
                #the probability is 1 divided by the number of actions the agent can take
                v += vSuccessor * (1.0/len(actions))

            return v

        actions = gameState.getLegalActions(0)
        #The base case for the recursion, start at the top of the tree, letting the mins act first.
        #Make a dictionary to keep track of each action and its value, return the action that has the max
        #value of the successor minValue values
        actionSet = {}
        for action in actions:
            actionSet[action] = expValue(gameState.generateSuccessor(0, action), 1, 1)
        return max(actionSet, key=actionSet.get)

def betterEvaluationFunction(currentGameState):
    """
    Your extreme ghost-hunting, pellet-nabbing, food-gobbling, unstoppable
    evaluation function (question 5).

    DESCRIPTION: <write something here so we know what you did>
    """
    "*** YOUR CODE HERE ***"
    # Useful information you can extract from a GameState (pacman.py)
    newPos = currentGameState.getPacmanPosition()
    newFood = currentGameState.getFood()
    newGhostStates = currentGameState.getGhostStates()
    newScaredTimes = [ghostState.scaredTimer for ghostState in newGhostStates]
    newCapsules = currentGameState.getCapsules()

    "*** YOUR CODE HERE ***"
    #Return a high value on a win
    if currentGameState.isWin():
        return 1000000

    #Return a low value on a loss
    if currentGameState.isLose():
        return -1000000

    value = 0

    #Find the closest food and closest ghost
    closestFood = min([manhattanDistance(newPos, food) for food in newFood.asList()])
    closestNewGhost = min([manhattanDistance(newPos, ghost.getPosition())
                           for ghost in newGhostStates])

    #If there are capsules, give them weight
    if len(newCapsules) > 0:
            closestCapsule = min([manhattanDistance(newPos, capsule) for capsule in newCapsules])
            value += float(1/closestCapsule)

    #Take reciprocal of closest food (closer foods better), and negatively weight states with
    #more remaining food
    value += float(1/closestFood)
    value -= currentGameState.getNumFood()

    

    #Negatively weight states that become closer to ghosts by taking the negative reciprocal.
    #If ghosts are scared, positively weight moving towards them instead
    if sum(newScaredTimes) < 0:
        value -= 1/closestNewGhost
    else:
        value += 1/closestNewGhost

    return currentGameState.getScore() + value

# Abbreviation
better = betterEvaluationFunction
